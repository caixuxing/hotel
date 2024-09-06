namespace Hotel.Application.TaskTrigger.Commands.SaveTaskTrigger;

/// <summary>
/// 任务触发设置Cmd
/// </summary>
public class SaveTaskTriggerCmd : IRequest<bool>
{
    /// <summary>
    /// 酒店Code
    /// </summary>
    public string BusinessId { get; set; } = null!;
    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime StartDate { get; set; }
    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime EndDate { get; set; }
    /// <summary>
    /// 溢价
    /// </summary>
    public decimal Premium { get; set; }
    /// <summary>
    /// 消息推送类型
    /// </summary>
    public int MsgPushType { get; set; }

    public bool Status { get; set; }
}
