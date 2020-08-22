using System;
using System.Collections.Generic;
using System.Text;

namespace Sino.LogisticsWorkOrderManage.Core
{
    /// <summary>
    /// 添加机器请求对象
    /// </summary>
    public class AddMachineryInfoRequest
    {
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
    }

    /// <summary>
    /// 添加机器请求对象
    /// </summary>
    public class MachineryInfoSearchRequest
    {
        /// <summary>
        /// 用户外键
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        /// 机器名称
        /// </summary>
        public string LikeName { get; set; }

        /// <summary>
        /// 购买时间
        /// </summary>
        public DateTime? PurchaseDate { get; set; }
    }
}
