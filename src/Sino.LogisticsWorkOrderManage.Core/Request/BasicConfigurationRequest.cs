using System;
using System.Collections.Generic;
using System.Text;

namespace Sino.LogisticsWorkOrderManage.Core
{
    /// <summary>
    /// 基础配置查询
    /// </summary>
    public class BasicConfigurationRequest
    {
        /// <summary>
        /// 数据类型，1：机器类型，2：机器品牌，3：主营业务，4：主营产品
        /// </summary>
        public int? DataType { get; set; }

        /// <summary>
        /// 配置名称（模糊）
        /// </summary>
        public string LikeCfgName { get; set; }

        /// <summary>
        /// 配置名称
        /// </summary>
        public string CfgName { get; set; }
    }
}
