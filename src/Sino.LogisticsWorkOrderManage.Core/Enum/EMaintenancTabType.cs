using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Sino.LogisticsWorkOrderManage.Core
{
    /// <summary>
    /// 维修单状态
    /// </summary>
    public enum  EMaintenancTabType
    {
        /// <summary>
        /// 缺省值
        /// </summary>
        [Description("缺省值")]
        None,

        /// <summary>
        /// 待受理
        /// </summary>
        [Description("待受理")]
        ToBeAccepted,

        /// <summary>
        /// 受理中
        /// </summary>
        [Description("受理中")]
        Acceptance,

        /// <summary>
        /// 已受理
        /// </summary>
        [Description("已受理")]
        Accepted,

        /// <summary>
        /// 已关闭
        /// </summary>
        [Description("已关闭")]
        Closed
    }
}
