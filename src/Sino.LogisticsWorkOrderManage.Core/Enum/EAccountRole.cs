using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Sino.LogisticsWorkOrderManage.Core
{
    public enum EAccountRole
    {
        /// <summary>
        /// 管理员
        /// </summary>
        [Description("管理员")]
        administrator = 0,

        /// <summary>
        /// 客户
        /// </summary>
        [Description("客户")]
        User = 1,

        /// <summary>
        /// 服务商
        /// </summary>
        [Description("服务商")]
        ServiceBusiness = 2,

        /// <summary>
        /// 维修员
        /// </summary>
        [Description("维修员")]
        RepairMan = 3
    }
}
