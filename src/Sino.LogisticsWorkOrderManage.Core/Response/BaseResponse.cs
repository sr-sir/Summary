using System;
using System.Collections.Generic;
using System.Text;

namespace Sino.LogisticsWorkOrderManage.Core
{
    public class BaseResponse<T>
        where T : class, new()
    {
        /// <summary>
        /// 数据集合
        /// </summary>
        public List<T> Items { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalCount { get; set; }
    }
}
