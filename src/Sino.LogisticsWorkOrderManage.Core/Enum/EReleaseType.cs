using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Sino.LogisticsWorkOrderManage.Core
{
    /// <summary>
    /// 发布方式 
    /// </summary>
    public enum EReleaseType
    {
        /// <summary>
        /// 缺省值
        /// </summary>
        [Description("缺省值")]
        None,

        /// <summary>
        /// 公开
        /// </summary>
        [Description("公开")]
        Public,

        /// <summary>
        /// 服务商
        /// </summary>
        [Description("服务商")]
        Manufactor,

        /// <summary>
        /// 指定
        /// </summary>
        [Description("指定")]
        Appoint
    }
}
