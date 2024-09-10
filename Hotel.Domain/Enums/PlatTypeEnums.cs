using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Domain.Enums
{
    /// <summary>
    /// 平台枚举
    /// </summary>
    public enum PlatTypeEnums
    {
        /// <summary>
        /// EXP
        /// </summary>
        [Description("EXP")]
        EXP = 1,
        /// <summary>
        /// 同程
        /// </summary>
        [Description("同程")]
        SameTrip = 2,
        /// <summary>
        /// 猫头鹰
        /// </summary>
        [Description("猫头鹰")]
        owl = 3,

        /// <summary>
        /// Agoda
        /// </summary>
        [Description("Agoda")]
        Agoda = 5,

        /// <summary>
        /// 去哪
        /// </summary>
        [Description("去哪儿")]
        WhereTo = 6
    }
}
