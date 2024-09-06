using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Domain.Enums
{
    /// <summary>
    /// 通讯类型
    /// </summary>
    public enum PushType
    {
        /// <summary>
        /// 邮件
        /// </summary>
        Mail=1,

        /// <summary>
        /// 钉钉
        /// </summary>
        DingTalk=2,

        /// <summary>
        /// 短信
        /// </summary>
        Sms=3

    }
}
