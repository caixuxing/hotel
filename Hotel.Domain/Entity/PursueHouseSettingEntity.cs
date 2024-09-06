using Hotel.Domain.Enums;
using SqlSugar;

namespace Hotel.Domain.Entity;

[SugarTable("PursueHouseSetting", "追房设置")]
[SugarIndex("index_{table}_C", nameof(CreateDate), OrderByType.Asc)]
public partial class PursueHouseSettingEntity
{
    /// <summary>
    /// 主键ID
    /// </summary>
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
    public int Id { get; init; }

    /// <summary>
    /// 业务ID
    /// </summary>
    public string BusinessId { get;private set; }
    /// <summary>
    /// 状态
    /// </summary>
    public TriggerStatus Status { get; private set; }

    /// <summary>
    /// 溢价
    /// </summary>
    [SugarColumn(IsNullable = true)]
    public decimal? Premium { get; private set; }

    /// <summary>
    /// 开始时间
    /// </summary>
    [SugarColumn(IsNullable = true)]
    public DateTime? StartDate { get; private set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    [SugarColumn(IsNullable = true)]
    public DateTime? EndDate { get; private set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateDate { get; private set; }
    /// <summary>
    /// 消息推送类型
    /// </summary>
    public PushType MsgPushType { get; private set; } = PushType.Mail;
}


public partial class PursueHouseSettingEntity
{
    public PursueHouseSettingEntity()
    { }

    public PursueHouseSettingEntity(int id, string businessId, TriggerStatus status, decimal? premium, 
        DateTime? startDate, DateTime? endDate, DateTime createDate, PushType msgPushType)
    {
        Id = id;
        BusinessId = businessId;
        Status = status;
        Premium = premium;
        StartDate = startDate;
        EndDate = endDate;
        CreateDate = createDate;
        MsgPushType = msgPushType;
    }



    /// <summary>
    /// 创建追房设置
    /// </summary>
    /// <param name="businessId"></param>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <returns></returns>
    public static PursueHouseSettingEntity Create(string businessId, TriggerStatus status, decimal? premium, DateTime? startDate, DateTime? endDate,
         PushType msgPushType)
    {
        return new(0, businessId, status, premium, startDate, endDate, DateTime.Now, msgPushType);
    }
 

    public PursueHouseSettingEntity SetStartDate(DateTime? date)
    {
        StartDate = date;
        return this;
    }
    public PursueHouseSettingEntity SetEndDate(DateTime? date)
    {
        this.EndDate = date;
        return this;
    }
    public PursueHouseSettingEntity SetPremium(decimal? premium)
    {
        this.Premium = premium??0m;
        return this;
    }

    public PursueHouseSettingEntity SetMsgPushType(PushType pushType)
    {
        this.MsgPushType = pushType;
        return this;
    }

}