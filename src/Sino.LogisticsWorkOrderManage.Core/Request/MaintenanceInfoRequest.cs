using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sino.LogisticsWorkOrderManage.Core
{
    /// <summary>
    /// 发布工单请求对象
    /// </summary>
    public class AddMaintenanceInfoRequest
    {
        /// <summary>
        /// 发布方式
        /// </summary>
        public EReleaseType ReleaseType { get; set; }

        /// <summary>
        /// 工单类型 
        /// </summary>
        public EMaintenancType MaintenancType { get; set; }

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
        /// 设备ID
        /// </summary>
        public string DeviceID { get; set; } = string.Empty;

        /// <summary>
        /// 购买时间
        /// </summary>
        public DateTime? PurchaseDate { get; set; }

        /// <summary>
        /// 故障类型
        /// </summary>
        public string FaultType { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; } = string.Empty;

        /// <summary>
        /// 用户外键
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        /// 机主姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string UserPhone { get; set; }

        /// <summary>
        /// 机主地址
        /// </summary>
        public string UserAddress { get; set; }

        /// <summary>
        ///机主省
        /// </summary>
        public string UserProvince { get; set; }

        /// <summary>
        /// 机主市
        /// </summary>
        public string UserCity { get; set; }

        /// <summary>
        /// 机主区
        /// </summary>
        public string UserCounty { get; set; }

        /// <summary>
        /// 服务商Id
        /// </summary>
        public Guid? RepairId { get; set; }

        /// <summary>
        /// 维修站名称
        /// </summary>
        public string RepairName { get; set; } = string.Empty;

        /// <summary>
        /// 维修站编号
        /// </summary>
        public string RepairNo { get; set; } = string.Empty;

        /// <summary>
        /// 维修站地址
        /// </summary>
        public string RepairAddress { get; set; } = string.Empty;

        /// <summary>
        /// 纬度值
        /// </summary>
        public string Lat { get; set; }

        /// <summary>
        /// 经度值
        /// </summary>
        public string Lng { get; set; }

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
        /// 附件列表
        /// </summary>
        public List<AttachmentItemRequest> AttachmentList { get; set; }

        /// <summary>
        /// 账户外键ID
        /// </summary>
        public string AccountinfoId { get; set; }
    }

    /// <summary>
    /// 客户工单查询对象
    /// </summary>
    public class MaintenanceInfoSearchRequest
    {
        /// <summary>
        /// 工单Id
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// 用户外键
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        /// 角色信息：0、管理员   1、客户  2、服务商 3、维修员
        /// </summary>
        public int? RoleId { get; set; }

        /// <summary>
        /// 1:待受理，2：已受理，3：已受理，4：已关闭
        /// </summary>
        public int? MaintenancType { get; set; }

        /// <summary>
        /// 服务商Id
        /// </summary>
        public Guid? RepairId { get; set; }

        /// <summary>
        /// 维修员Id
        /// </summary>
        public Guid? RepairManId { get; set; }
    }

    public class MaintenanceInfoPageRequest : MaintenanceInfoSearchRequest
    {

        public int? Skip { get; set; }

        public int? Count { get; set; }
    }

    /// <summary>
    /// 工单指派维修员
    /// </summary>
    public class MaintenanceAssignRpRequest
    {
        /// <summary>
        /// 维修员Id
        /// </summary>
        [Required(ErrorMessage = "请选择维修员")]
        public Guid? RepairManId { get; set; }

        /// <summary>
        /// 工单Id
        /// </summary>
        [Required(ErrorMessage ="请提供工单Id")]
        public Guid? Id { get; set; }
    }

    /// <summary>
    /// 服务商取消工单
    /// </summary>
    public class SPCancelMaintenanceRequest
    {
        /// <summary>
        /// 工单Id
        /// </summary>
        [Required(ErrorMessage = "请提供工单Id")]
        public Guid? Id { get; set; }
    }

    /// <summary>
    /// 服务商领取工单
    /// </summary>
    public class SPReceiveMaintenanceRequest
    {
        /// <summary>
        /// 工单Id
        /// </summary>
        [Required(ErrorMessage = "请提供工单Id")]
        public Guid? Id { get; set; }
    }

    /// <summary>
    /// 工单转派
    /// </summary>
    public class TransformAssignRequest
    {
        /// <summary>
        /// 工单Id
        /// </summary>
        [Required(ErrorMessage = "请提供工单Id")]
        public Guid? Id { get; set; }

        /// <summary>
        /// 新的维修员Id
        /// </summary>
        [Required(ErrorMessage = "请选择维修员")]
        public Guid? NewRepairManId { get; set; }
    }

    /// <summary>
    /// 维修员受理工单
    /// </summary>
    public class RPReceiveMaintenanceRequest
    {
        /// <summary>
        /// 工单Id
        /// </summary>
        [Required(ErrorMessage = "请提供工单Id")]
        public Guid? Id { get; set; }
    }
}
