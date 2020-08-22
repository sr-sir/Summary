using System;
using Sino.Domain.Entities;


namespace Sino.LogisticsWorkOrderManage.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class AddressInfo: Entity<long>
    {
        /// <summary>
        /// 
        /// </summary>
        public AddressInfo()
        {
        }


        /// <summary>
        /// 用户外键
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        /// 省
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// 市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 区
        /// </summary>
        public string County { get; set; }

        /// <summary>
        /// 详细地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreationTime { get; set; }

        /// <summary>
        /// 创建用户Id
        /// </summary>
        public Guid? CreatorUserId { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool? IsDeleted { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>
        public DateTime? DeletionTime { get; set; }

        /// <summary>
        /// 删除用户
        /// </summary>
        public Guid? DeleterUserId { get; set; }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime? LastModificationTime { get; set; }

        /// <summary>
        /// 最后修改人
        /// </summary>
        public Guid? LastModifierUserId { get; set; }
    }
}