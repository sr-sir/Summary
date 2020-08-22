using System;
using Sino.Domain.Entities;


namespace Sino.LogisticsWorkOrderManage.Core
{
    /// <summary>
    /// 客户机器
    /// </summary>
    public class MachineryInfo: Entity<long>
    {
        /// <summary>
        /// 
        /// </summary>
        public MachineryInfo()
        {
        }

        /// <summary>
        /// 用户外键
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        /// 农机名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
        public string DeviceID { get; set; }

        /// <summary>
        /// 农机类型外键
        /// </summary>
        public string AgriculturalId { get; set; }

        /// <summary>
        /// 农机类型名称
        /// </summary>
        public string AgriculturalName { get; set; }

        /// <summary>
        /// 农机品牌
        /// </summary>
        public string AgriculturalBrand { get; set; }

        /// <summary>
        /// 购买时间
        /// </summary>
        public DateTime? PurchaseDate { get; set; }

        /// <summary>
        /// 农机图片
        /// </summary>
        public string ImgUrl { get; set; }

        /// <summary>
        /// 出厂编号
        /// </summary>
        public string FactoryNumber { get; set; }

        /// <summary>
        /// 机器类型Id
        /// </summary>
        public int? AgriculturalTypeId { get; set; }

        /// <summary>
        /// 机器品牌Id
        /// </summary>
        public int? AgriculturalBrandId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }

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