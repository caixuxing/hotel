using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.TaskTrigger.Query
{
    public class FindPursueHouseSettingDto
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int Id { get; init; }

        /// <summary>
        /// 业务ID
        /// </summary>
        public string BusinessId { get;  set; }
        /// <summary>
        /// 状态
        /// </summary>
        public TriggerStatus Status { get;  set; }

        /// <summary>
        /// 溢价
        /// </summary>
        public decimal? Premium { get;  set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartDate { get;  set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndDate { get;  set; }

        /// <summary>
        /// 消息推送类型
        /// </summary>
        public PushType? MsgPushType { get;  set; }
    }
}
