using Hotel.Domain.EntityMG;
using Hotel.Domain.ValueObject;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using Newtonsoft.Json;
using Snowflake.Core;
using System.Collections.Generic;

namespace Hotel.Application.TaskTrigger.Commands.SaveTaskTrigger;

internal sealed class SaveTaskTriggerHandler : IRequestHandler<SaveTaskTriggerCmd, bool>
{
    readonly ISqlSugarClient _db;

     readonly IdWorker _idWorker;

    readonly IPursueHouseRecordRepo _recordRepo;

    public SaveTaskTriggerHandler(ISqlSugarClient db, IdWorker idWorker, IPursueHouseRecordRepo recordRepo )
    {
        _db = db;
        _idWorker = idWorker;
        _recordRepo = recordRepo;
    }

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

                await pursueHouseSettingRepo.UpdateAsync(data);
                //如果是更新  清空原来追房任务计划记录
                await _recordRepo.DeletePursueHouseRecord(x=>x.TriggerId==id);
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
                id = await pursueHouseSettingRepo.InsertReturnIdentityAsync(data);
            }

            var arHotel = _db.Queryable<ARHotelEntity>()
                 .Where(x => x.HotelCode == request.BusinessId)
                 .Select(x => new ARHotelObj { OtherPlatType = x.OtherPlatType, OtherHotelCode = x.OtherHotelCode! })
                 .ToList();
            if (!arHotel.Any())
            {
                throw new Exception("请先配置酒店的映射平台信息！");
            }
            //var triggerRecord = PursueHouseRecordEntity.BuildTaskDetail(
            //    triggerId: id,
            //    startDate: request.StartDate,
            //    endDate: request.EndDate,
            //    businessId: request.BusinessId,
            //    premium: request.Premium,
            //    msgPushType: (PushType)Enum.Parse(typeof(PushType), request.MsgPushType.ToString()),
            //   JsonConvert.DeserializeObject(JsonConvert.SerializeObject(arHotel))
            //    ).ToList();
            ////插入追房任务计划
            //await pursueHouseRecordRepo.InsertRangeAsync(triggerRecord);

            var pursueHouseRecordData = PursueHouseRecord.BuildTaskDetail(
                no: _idWorker,
                triggerId: id,
                startDate: request.StartDate,
                endDate: request.EndDate,
                businessId: request.BusinessId,
                premium: request.Premium,
                msgPushType: (PushType)Enum.Parse(typeof(PushType), request.MsgPushType.ToString()),
               arHotel
                ).ToList();
            await _recordRepo.AddPursueHouseRecord(pursueHouseRecordData);
            context.Commit();
        }
        return true;
    }
}
