using System;
using Sino.Domain.Entities;


namespace Sino.LogisticsWorkOrderManage.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class BasicConfiguration: Entity<int>
    {
        /// <summary>
        /// 
        /// </summary>
        public BasicConfiguration()
        {

        }


        /// <summary>
        /// 数据类型，1：机器类型，2：机器品牌，3：主营业务，4：主营产品
        /// </summary>
        public int? DataType { get; set; }

        /// <summary>
        /// 配置名称
        /// </summary>
        public string CfgName { get; set; }

        /// <summary>
        /// 配置值
        /// </summary>
        public string CfgVal { get; set; }

        /// <summary>
        /// 配置编码
        /// </summary>
        public string CfgCode { get; set; }

        /// <summary>
        /// 备用，父类Id
        /// </summary>
        public int? ParentId { get; set; }

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