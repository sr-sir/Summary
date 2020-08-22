using System;
using Sino.Domain.Entities;


namespace Sino.LogisticsWorkOrderManage.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class MaintenanceInfo : Entity<Guid>
    {
        /// <summary>
        /// 
        /// </summary>
        public MaintenanceInfo()
        {
        }

        /// <summary>
        /// 维修单号
        /// </summary>
        public string Numbers { get; set; }

        /// <summary>
        /// 维修方式
        /// </summary>
        public int? ReleaseType { get; set; }

        /// <summary>
        /// 工单类型
        /// </summary>
        public int? MaintenancType { get; set; }

        /// <summary>
        /// 工单状态
        /// </summary>
        public int? MaintenancStatus { get; set; }

        /// <summary>
        /// 农机类型Id
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
        /// 设备ID
        /// </summary>
        public string DeviceID { get; set; }

        /// <summary>
        /// 购买时间
        /// </summary>
        public DateTime? PurchaseDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string FaultType { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        /// 机主名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 机主号码
        /// </summary>
        public string UserPhone { get; set; }

        /// <summary>
        /// 详细地址
        /// </summary>
        public string UserAddress { get; set; }

        /// <summary>
        /// 省份
        /// </summary>
        public string UserProvince { get; set; }

        /// <summary>
        /// 市
        /// </summary>
        public string UserCity { get; set; }

        /// <summary>
        /// 区
        /// </summary>
        public string UserCounty { get; set; }

        /// <summary>
        /// 维修站Id（服务商Id）
        /// </summary>
        public Guid? RepairId { get; set; }

        /// <summary>
        /// 维修员Id
        /// </summary>
        public Guid? RepairmanId { get; set; }

        /// <summary>
        /// 维修员名称
        /// </summary>
        public string RepairmanName { get; set; }

        /// <summary>
        /// 维修员号码
        /// </summary>
        public string RepairmanPhone { get; set; }

        /// <summary>
        /// 维修站名称
        /// </summary>
        public string RepairName { get; set; }

        /// <summary>
        /// 维修站编号
        /// </summary>
        public string RepairNo { get; set; }

        /// <summary>
        /// 维修站地址
        /// </summary>
        public string RepairAddress { get; set; }

        /// <summary>
        /// 接单时间
        /// </summary>
        public DateTime? ReceiptTime { get; set; }

        /// <summary>
        /// 完成时间
        /// </summary>
        public DateTime? CompleteTime { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreationTime { get; set; }

        /// <summary>
        /// 纬度值
        /// </summary>
        public string Lat { get; set; }

        /// <summary>
        /// 经度值
        /// </summary>
        public string Lng { get; set; }

        /// <summary>
        /// 农机型号
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

        public Guid? CreatorUserId { get; set; }

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