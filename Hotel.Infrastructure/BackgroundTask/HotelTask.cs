using Hotel.Domain.EntityMG;
using Hotel.Domain.Shared.Const;
using Hotel.Domain.Shared.Response;
using Hotel.Domain.ValueObject;
using Microsoft.Extensions.Hosting;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using Hotel.Domain.Shared.Utils;

namespace Hotel.Infrastructure.BackgroundTask;

internal class HotelTask : BackgroundService
{
    readonly IServiceScopeFactory _scopeFactory;
    readonly IHttpClientFactory _httpClientFactory;
    private Queue<PursueHouseRecordDto> taskQueue = new();
    private static readonly Lazy<Dictionary<PlatTypeEnums, string>> _InitData = new(() => new() {
            {PlatTypeEnums.EXP, "/spider/expediataap"},
            {PlatTypeEnums.SameTrip, "/spider/elong"},
            {PlatTypeEnums.owl, "/spider/tripadvisor"},
            {PlatTypeEnums.Agoda, "/spider/agoda"},
            {PlatTypeEnums.WhereTo, "/spider/qunar"}
    });

    public HotelTask(IServiceScopeFactory scopeFactory, IHttpClientFactory httpClientFactory)
    {
        _scopeFactory = scopeFactory;
        _httpClientFactory = httpClientFactory;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await DoWorkAsync(stoppingToken);
                //休眠30秒
                await Task.Delay(30000, stoppingToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"后台任务执行异常：{ex.Message}");
            }

        }
        
    }

    /// <summary>
    /// 执行工作任务
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private async Task DoWorkAsync(CancellationToken cancellationToken)
    {
        using var serviceScope = _scopeFactory.CreateScope();
        var _db = serviceScope.ServiceProvider.GetRequiredService<ISqlSugarClient>();
        var _PursueHouseRecordRepo = serviceScope.ServiceProvider.GetRequiredService<IPursueHouseRecordRepo>();
        var _taskExecCursorRepo = serviceScope.ServiceProvider.GetRequiredService<ITaskExecCursorRepo>();
        var taskExecCursorData = await _taskExecCursorRepo.GetTaskExecCursorById(new ObjectId(GlobaConst.TaskExecCursorObjId));
        int QueueLength = await this.TaskQueuePool(serviceScope, taskExecCursorData);
        if (QueueLength < 1) return;
        var queue = taskQueue.Dequeue();
        var oneItem = queue?.ARHotelObjs?.SingleOrDefault(x => x.OtherPlatType == PlatTypeEnums.WhereTo.GetHashCode().ToString());
        if (oneItem is not null)
        {
            //优先查询去哪,把去哪儿平台插入到第一位
            queue?.ARHotelObjs?.Remove(oneItem!);
            queue?.ARHotelObjs?.Insert(0, oneItem!);
        }
        foreach (var i in queue?.ARHotelObjs ?? new())
        {
            PlatTypeEnums otherPlatTypeEnums=(PlatTypeEnums)Enum.Parse(typeof(PlatTypeEnums), i.OtherPlatType);
            var url = _InitData.Value.SingleOrDefault(x => x.Key == otherPlatTypeEnums).Value;
            var client = _httpClientFactory.CreateClient("zicp");
            var paramData = queue?.ARHotelObjs?.SingleOrDefault(x => x.OtherPlatType == i.OtherPlatType);
            var param = new
            {
                hotel_id = paramData?.OtherHotelCode,
                check_in_date = queue?.CurrentDate.ToString("yyyy-MM-dd"),
                check_out_date = queue?.EndDate.ToString("yyyy-MM-dd")
            };
            taskExecCursorData.RunId = queue?.No ?? 0;
            Console.WriteLine($"执行任务计划编号:{taskExecCursorData.RunId}-酒店编码:{queue?.BusinessId}-平台类型：{otherPlatTypeEnums.ToDescription()}");
            var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);
            var result = await response.Content.ReadAsStringAsync();
            var data = StrFormat<ResultJson<List<HotelData>>>(result,url);
            var lowestPriceItem = data?.Data?.Where(item => data?.Data?.Min(i => i.Price) == item.Price).FirstOrDefault();
            //最低价大于溢价跳出遍历
            if (lowestPriceItem?.Price > queue?.Premium)
            {
                //ToDo:推送信息通知
                Console.WriteLine($"{otherPlatTypeEnums.ToDescription()}:推送信息通知");
                await Task.Delay(5000);
                //跳出其它平台查价
                break;
            }
        }
        await _taskExecCursorRepo.UpdateTaskExecCursor(taskExecCursorData);
    }

    /// <summary>
    /// 字符串格式化
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="result"></param>
    /// <param name="url"></param>
    /// <returns></returns>
    public T? StrFormat<T>(string result,string url)
    {
        try
        {
            return JsonConvert.DeserializeObject<T>(result);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"远程请求地址:{url}解析异常错误信息:{ex.Message}");
            return default(T);
        }
    }

    /// <summary>
    /// 任务队列池
    /// </summary>
    async Task<int> TaskQueuePool(IServiceScope serviceScope, TaskExecCursor taskExecCursorData)
    {
        var _PursueHouseRecordRepo = serviceScope.ServiceProvider.GetRequiredService<IPursueHouseRecordRepo>();
        if (!taskQueue.Any())
        {
            Console.WriteLine($"根据ID:【{taskExecCursorData.TakeId}】从数据库中捞取任务计划数据追加到任务队列池中");
            var data = _PursueHouseRecordRepo.GetListAsync(x => x.No > taskExecCursorData.RunId &&
                                                        x.Status == TriggerStatus.Enable && 
                                                        x.currentDate >= DateTime.Now)
                              .Select(x => new PursueHouseRecordDto
                              {
                                  No = x.No,
                                  BusinessId = x.BusinessId ?? string.Empty,
                                  CurrentDate = x.currentDate!.Value,
                                  EndDate=x.EndDate!.Value,
                                  ARHotelObjs = x.HotelIdCode
                              }).ToList();
            if (data?.Count == 0)
            {
               await Task.Delay(999999999);
                Console.WriteLine("===============本轮所有任务计划已执行完成,即将开启下一轮任务计划执行!===============");
                taskExecCursorData.TakeId = 0;
                taskExecCursorData.RunId = 0;
                var taskExecCursorRepo = serviceScope.ServiceProvider.GetRequiredService<ITaskExecCursorRepo>();
                await taskExecCursorRepo.UpdateTaskExecCursor(taskExecCursorData);
                return 0;
            }
            data?.ForEach(item=> taskQueue.Enqueue(item));
            taskExecCursorData.TakeId = taskQueue.Last().No;
        }
        return taskQueue.Count;
    }
}






public class PursueHouseRecordDto
{
    public long No { get; set; }

    public string? BusinessId { get; set; }

    public DateTime CurrentDate { get; set; }

    public DateTime EndDate { get; set; }

    public decimal Premium { get; set; }

    public List<ARHotelObj>? ARHotelObjs { get; set; }
}

public class HotelData
{
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
}
