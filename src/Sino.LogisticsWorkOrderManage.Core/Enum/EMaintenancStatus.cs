using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Sino.LogisticsWorkOrderManage.Core
{
    /// <summary>
    /// 工单状态
    /// </summary>
    public enum EMaintenancStatus
    {
        //维修单状态 1：未指派（未指派服务商），2：已指派（已指派服务商），3：已领取（已被服务商领取），
        //4：已受理（已被服务商指派维修员），5：受理中（维修员已受理工单任务），6：已完成（维修员工单维修完成），7：已关闭

        /// <summary>
        /// 1：未指派（未指派服务商）
        /// </summary>
        [Description("未指派")]
        UnAssign = 1,
        /// <summary>
        /// 已指派（已指派服务商）
        /// </summary>
        [Description("已指派")]
        Assigned = 2,
        /// <summary>
        /// 已领取（已被服务商领取）
        /// </summary>
        [Description("已领取")]
        Received = 3,
        /// <summary>
        /// 已受理（已被服务商指派维修员）
        /// </summary>
        [Description("已受理")]
        AssignRP = 4,
        /// <summary>
        /// 受理中（维修员已受理工单任务）
        /// </summary>
        [Description("受理中")]
        Accepting = 5,
        /// <summary>
        /// 已完成（维修员工单维修完成）
        /// </summary>
        [Description("已完成")]
        Complete = 6,
        /// <summary>
        /// 已关闭
        /// </summary>
        [Description("已关闭")]
        Closed = 7
    }
}
