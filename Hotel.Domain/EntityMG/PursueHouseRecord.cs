using Hotel.Domain.Enums;
using Hotel.Domain.Extend;
using Hotel.Domain.ValueObject;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json.Linq;
using Snowflake.Core;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Domain.EntityMG
{
    /// <summary>
    ///追房运行记录表
    /// </summary>

    public partial class PursueHouseRecord
    {

        public ObjectId Id { get; set; }

        public long No { get; set; }

        /// <summary>
        /// 触发器Id
        /// </summary>
        public int TriggerId { get; set; }

        /// <summary>
        /// 当前运行次数
        /// </summary>
        public long NumberOfRuns { get; set; }

        /// <summary>
        /// 最近运行时间
        /// </summary>
        public DateTime? LastRunTime { get; set; }

        /// <summary>
        /// 触发器状态
        /// </summary>
        public TriggerStatus Status { get; set; } = TriggerStatus.Enable;

        /// <summary>
        /// 本次执行结果
        /// </summary>
        [MaxLength(128)]
        public string? Result { get; set; }

        /// <summary>
        /// 本次执行耗时
        /// </summary>
        public long ElapsedTime { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreatedTime { get; set; }


        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartDate { get; private set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndDate { get; private set; }

        /// <summary>
        /// 当前日期
        /// </summary>
        public DateTime? currentDate { get; private set; }

        /// <summary>
        /// 业务ID
        /// </summary>
        public string? BusinessId { get; set; }

        /// <summary>
        /// 溢价
        /// </summary>
        public decimal? Premium { get; set; }

        /// <summary>
        /// 消息推送类型
        /// </summary>
        public PushType MsgPushType { get; set; } = PushType.Mail;

        /// <summary>
        /// 平台酒店ID编码(平台映射OtherHotelCode字段值)Json
        /// </summary>
        public List<ARHotelObj> HotelIdCode { get; set; }

    }


    

    public partial class PursueHouseRecord
    {
        public PursueHouseRecord()
        { }

        private PursueHouseRecord( long no,int triggerId, long numberOfRuns, DateTime? lastRunTime,
            TriggerStatus status, string? result, long elapsedTime, DateTime? createdTime, DateTime? startDate, DateTime? endDate,
            DateTime? currentDate, string? businessId, decimal? premium, PushType msgPushType, List<ARHotelObj> hotelIdCode)
        {
            No = no;
            TriggerId = triggerId;
            NumberOfRuns = numberOfRuns;
            LastRunTime = lastRunTime;
            Status = status;
            Result = result;
            ElapsedTime = elapsedTime;
            CreatedTime = createdTime;
            StartDate = startDate;
            EndDate = endDate;
            this.currentDate = currentDate;
            BusinessId = businessId;
            Premium = premium;
            MsgPushType = msgPushType;
            this.HotelIdCode = hotelIdCode;
        }

        /// <summary>
        /// 生成任务计划记录详细
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static IEnumerable<PursueHouseRecord> BuildTaskDetail(
            IdWorker no,
            int triggerId,
            DateTime startDate,
            DateTime endDate,
            string? businessId,
            decimal? premium,
            PushType msgPushType,
            List<ARHotelObj> hotelIdCode
            )
        {
            for (DateTime date = startDate; date < endDate; date = date.AddDays(1))
            {
                yield return new PursueHouseRecord(
                    no.NextId(),
                    triggerId: triggerId,
                    numberOfRuns: 0,
                    status: TriggerStatus.Enable,
                    result: null,
                    lastRunTime: null,
                    elapsedTime: 0,
                    createdTime: DateTime.Now,
                    startDate: startDate.Date,
                    endDate: endDate.Date,
                    currentDate: date.Date,
                    businessId: businessId,
                    premium: premium,
                    msgPushType: msgPushType,
                    hotelIdCode: hotelIdCode
                    );
            }
        }
    }
}
