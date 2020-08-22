using System;
using Sino.Domain.Entities;


namespace Sino.LogisticsWorkOrderManage.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class Attachment : Entity<long>
    {
        /// <summary>
        /// 
        /// </summary>
        public Attachment()
        {

        }


        /// <summary>
        /// 附近名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 附件地址
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 1:工单
        /// </summary>
        public int? DataType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid? DataId { get; set; }

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