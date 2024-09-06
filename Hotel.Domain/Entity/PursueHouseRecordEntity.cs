using Hotel.Domain.Enums;
using Hotel.Domain.Extend;
using Newtonsoft.Json;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Domain.Entity
{
    /// <summary>
    ///追房运行记录表
    /// </summary>
    [SugarTable("PursueHouseRecord", "追房运行记录表")]
    [SugarIndex("index_{table}_C", nameof(CreatedTime), OrderByType.Asc)]
    public partial class PursueHouseRecordEntity
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 触发器Id
        /// </summary>
        [SugarColumn(ColumnDescription = "触发器Id")]
        public int TriggerId { get; set; }

        /// <summary>
        /// 当前运行次数
        /// </summary>
        [SugarColumn(ColumnDescription = "当前运行次数", IsNullable = true)]
        public long NumberOfRuns { get; set; }

        /// <summary>
        /// 最近运行时间
        /// </summary>
        [SugarColumn(ColumnDescription = "最近运行时间", IsNullable = true)]
        public DateTime? LastRunTime { get; set; }

        /// <summary>
        /// 触发器状态
        /// </summary>
        [SugarColumn(ColumnDescription = "触发器状态")]
        public TriggerStatus Status { get; set; } = TriggerStatus.Enable;

        /// <summary>
        /// 本次执行结果
        /// </summary>
        [SugarColumn(ColumnDescription = "本次执行结果", Length = 128, IsNullable = true)]
        [MaxLength(128)]
        public string? Result { get; set; }

        /// <summary>
        /// 本次执行耗时
        /// </summary>
        [SugarColumn(ColumnDescription = "本次执行耗时", IsNullable = true)]
        public long ElapsedTime { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(ColumnDescription = "创建时间")]
        public DateTime? CreatedTime { get; set; }


        /// <summary>
        /// 开始时间
        /// </summary>
        [SugarColumn(ColumnDescription = "开始时间", ColumnDataType = "date", IsNullable = true)]
        public DateTime? StartDate { get; private set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [SugarColumn(ColumnDescription = "结束时间", ColumnDataType = "date", IsNullable = true)]
        public DateTime? EndDate { get; private set; }

        /// <summary>
        /// 当前日期
        /// </summary>
        [SugarColumn(ColumnDescription = "当前日期", ColumnDataType ="date", IsNullable = true)]
        public DateTime? currentDate { get; private set; }

        /// <summary>
        /// 业务ID
        /// </summary>
        [SugarColumn(ColumnDescription = "业务ID", IsNullable = true)]
        public string? BusinessId { get; set; }

        /// <summary>
        /// 溢价
        /// </summary>
        [SugarColumn(ColumnDescription = "溢价", IsNullable = true)]
        public decimal? Premium { get; set; }

        /// <summary>
        /// 消息推送类型
        /// </summary>
        [SugarColumn(ColumnDescription = "消息推送类型", IsNullable = true)]
        public PushType MsgPushType { get; set; } = PushType.Mail;

        /// <summary>
        /// 平台酒店ID编码(平台映射OtherHotelCode字段值)Json
        /// </summary>
        [SugarColumn(ColumnDescription = "平台酒店ID编码(平台映射OtherHotelCode字段值)Json", ColumnDataType = "text",
            IsJson = true, IsNullable = true,SqlParameterDbType = typeof(JsonDynamicConverter)
            )]
        public dynamic? HotelIdCode { get; set; }
    }

    public partial class PursueHouseRecordEntity
    {
        public PursueHouseRecordEntity()
        { }

        private PursueHouseRecordEntity(int id, int triggerId, long numberOfRuns, DateTime? lastRunTime,
            TriggerStatus status, string? result, long elapsedTime, DateTime? createdTime, DateTime? startDate, DateTime? endDate, 
            DateTime? currentDate, string? businessId, decimal? premium, PushType msgPushType, dynamic? hotelIdCode)
        {
            Id = id;
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
        public static IEnumerable<PursueHouseRecordEntity> BuildTaskDetail(
            int triggerId,
            DateTime startDate,
            DateTime endDate,
            string? businessId,
            decimal? premium, 
            PushType msgPushType,
            dynamic? hotelIdCode)
        {
            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                yield return new PursueHouseRecordEntity(
                    id: 0,
                    triggerId: triggerId,
                    numberOfRuns: 0,
                    status: TriggerStatus.Enable,
                    result: null,
                    lastRunTime: null,
                    elapsedTime: 0,
                    createdTime: DateTime.Now,
                    startDate: startDate,
                    endDate: endDate,
                    currentDate: date,
                    businessId: businessId,
                    premium: premium,
                    msgPushType: msgPushType,
                    hotelIdCode: hotelIdCode
                    );
            }
        }
    }
}