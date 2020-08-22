using System;

namespace Sino.LogisticsWorkOrderManage.Core
{
    /// <summary>
    /// 删除数据接口
    /// </summary>
    public interface IDeleted
    {
        /// <summary>
        /// 是否删除
        /// </summary>
        bool? IsDeleted { get; set; }
    }

    public interface IOnlyDeleted
    {
        /// <summary>
        /// 是否删除
        /// </summary>
        bool IsDeleted { get; set; }
    }
}
