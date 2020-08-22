using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Sino.LogisticsWorkOrderManage.Core
{
    /// <summary>
    /// 工单类型
    /// </summary>
    public enum EMaintenancType
    {
        /// <summary>
        /// 部件故障
        /// </summary>
        [Description("部件故障")]
        ComponentFailure = 1,

        /// <summary>
        /// 配件损耗
        /// </summary>
        [Description("配件损耗")]
        AccessoriesLoss = 2,

        /// <summary>
        /// 轮胎故障
        /// </summary>
        [Description("轮胎故障")] 
        TireFailure = 3,

        /// <summary>
        /// 发动机故障
        /// </summary>
        [Description("发动机故障")]
        EngineFailure = 4
    }
}
