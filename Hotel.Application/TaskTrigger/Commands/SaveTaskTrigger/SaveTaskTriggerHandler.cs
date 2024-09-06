using Newtonsoft.Json;
using System.Collections.Generic;

namespace Hotel.Application.TaskTrigger.Commands.SaveTaskTrigger;

internal sealed class SaveTaskTriggerHandler : IRequestHandler<SaveTaskTriggerCmd, bool>
{
    readonly ISqlSugarClient _db;

    public SaveTaskTriggerHandler(ISqlSugarClient db)=> _db = db;

    public async Task<bool> Handle(SaveTaskTriggerCmd request, CancellationToken cancellationToken)
    {
        using (var context = _db.CreateContext())
        {
            int id = 0;
            var pursueHouseSettingRepo = context.GetRepository<PursueHouseSettingEntity>();
            var pursueHouseRecordRepo = context.GetRepository<PursueHouseRecordEntity>();

            var data = await pursueHouseSettingRepo.GetSingleAsync(x => x.BusinessId == request.BusinessId);
            if (data is not null)
            {
                id = data.Id;
                data.SetStartDate(request.StartDate)
                    .SetEndDate(request.EndDate)
                    .SetMsgPushType((PushType)Enum.Parse(typeof(PushType), request.MsgPushType.ToString()))
                    .SetPremium(request.Premium);
            }
            else
            {
                data = PursueHouseSettingEntity.Create(
                    request.BusinessId,
                   (TriggerStatus)Enum.Parse(typeof(TriggerStatus), (request.Status ? 1 : 0).ToString()),
                    request.Premium,
                    request.StartDate,
                    request.EndDate,
                    (PushType)Enum.Parse(typeof(PushType), request.MsgPushType.ToString()));
            }

            if (id == 0)
            {
                id = await pursueHouseSettingRepo.InsertReturnIdentityAsync(data);
            }
            else
            {
                await pursueHouseSettingRepo.UpdateAsync(data);
                //如果是更新  清空原来追房任务计划记录
                await pursueHouseRecordRepo.DeleteAsync(x => x.TriggerId == data.Id);
            }

            var arHotel = _db.Queryable<ARHotelEntity>()
                 .Where(x => x.HotelCode == request.BusinessId)
                 .Select(x => new { x.OtherPlatType, x.OtherHotelCode })
                 .ToList();
            if (!arHotel.Any())
            {
                throw new Exception("请先配置酒店的映射平台信息！");
            }




            var triggerRecord = PursueHouseRecordEntity.BuildTaskDetail(
                triggerId: id,
                startDate: request.StartDate,
                endDate: request.EndDate,
                businessId: request.BusinessId,
                premium: request.Premium,
                msgPushType: (PushType)Enum.Parse(typeof(PushType), request.MsgPushType.ToString()),
               JsonConvert.DeserializeObject(JsonConvert.SerializeObject(arHotel))
                ).ToList();
            //插入追房任务计划
            await pursueHouseRecordRepo.InsertRangeAsync(triggerRecord);
            context.Commit();
        }
        return true;
    }
}
