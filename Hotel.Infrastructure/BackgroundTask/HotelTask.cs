

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Hotel.Infrastructure.BackgroundTask;

internal class HotelTask : BackgroundService
{
     readonly IServiceScopeFactory _scopeFactory;
     readonly IHttpClientFactory _httpClientFactory;
     private Queue<PursueHouseRecordDto> taskQueue = new();
    private Dictionary<string, int> dict = new() {
            {"take",0},
            {"run", 0 }
    };

    public HotelTask(IServiceScopeFactory scopeFactory, IHttpClientFactory httpClientFactory)
    {
        _scopeFactory = scopeFactory;
        _httpClientFactory = httpClientFactory;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

        while (!stoppingToken.IsCancellationRequested)
        {
            await DoWorkAsync(stoppingToken);

            //休眠10秒
            await Task.Delay(10000, stoppingToken);
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
        int QueueLength= await this.TaskQueuePool(_db);
        if (QueueLength < 1) return;
        List<PursueHouseRecordDto> batch = new List<PursueHouseRecordDto>();
        int count = Math.Min(10, QueueLength);

        List<Task<string>> taskList=new List<Task<string>> ();
        for (int i = 0; i < count; i++)
        {
            var item = taskQueue.Dequeue();
            batch.Add(item);
            Console.WriteLine($"执行任务计划编号:{item.Id}-酒店编码:{item.BusinessId}");
            
            taskList.Add(Task.Run(async() =>
            {
                var client = _httpClientFactory.CreateClient("zicp");
                var param = new
                {
                    hotel_id = "victory-platinum-service-suites-klcc/hotel/kuala-lumpur-my.html",
                    check_in_date = "2024-09-05",
                    check_out_date = "2024-09-06"
                };
                /*              var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, "application/json");
                              var result = await client.PostAsync("/spider/agoda", content);
                              var datas = await result.Content.ReadAsStringAsync();*/
                //模拟耗时操作
                await Task.Delay(50);
                return "success";
            }));
        }
        //等待任务完成
        Task.WaitAll(taskList.ToArray());
        dict.Remove("run");
        dict["run"] = batch.Any()?batch.Last().Id:0;
        Console.WriteLine($"任务已处理完毕！{DateTime.Now}");
        await Task.Delay(1000);
    }


    /// <summary>
    /// 任务队列池
    /// </summary>
    async Task<int> TaskQueuePool(ISqlSugarClient _db)
    {
        if (!taskQueue.Any())
        {
            int id = (dict["run"] == dict["take"]) ? dict["take"] : dict["run"];
            Console.WriteLine($"根据ID:【{id}】从数据库中捞取任务计划数据追加到任务队列池中");
            var data = await _db.Queryable<PursueHouseRecordEntity>().Take(50).Where(x => x.Id > id && x.Status==TriggerStatus.Enable && x.currentDate>=DateTime.Now )
                .OrderBy(x => new { x.Id })
                .Select(x => new PursueHouseRecordDto {
                            Id = x.Id,
                            BusinessId = x.BusinessId ?? string.Empty,
                            CurrentDate = x.currentDate!.Value
                        }).ToListAsync();
            if (data?.Count == 0)
            {
                Console.WriteLine("本轮所有任务计划已执行完成!");
                Console.WriteLine("即将开启下一轮任务计划执行!");

                dict.Remove("take");
                dict["take"] = 0;
                dict.Remove("run");
                dict["run"] = 0;
                return 0;
            }
            data?.ForEach((item) => taskQueue.Enqueue(item));
            //记录当前
            dict.Remove("take");
            dict["take"] = taskQueue.Last().Id;
        }
        return taskQueue.Count;
    }
}






public class PursueHouseRecordDto
{
    public int Id { get; set; }

    public string BusinessId { get; set; }

    public DateTime CurrentDate { get; set; }
}
