using Sino.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sino.LogisticsWorkOrderManage.Core.Models
{
    public class ServiceBusiness : Entity<Guid>
    {
        /// <summary>
        /// 服务商编号 
        /// </summary>
        public int ServiceBusinessNo { get; set; }

        /// <summary>
        /// 服务商名称
        /// </summary>
        public string ServiceBusinessName { get; set; }

        /// <summary>
        /// 服务商类型：1、金牌服务商  2、一般服务商
        /// </summary>
        public int ServiceBusinessType { get; set; }

        /// <summary>
        /// 主营业务
        /// </summary>
        public string MainBusiness { get; set; }

        /// <summary>
        /// 主营产品
        /// </summary>
        public string MainProducts { get; set; }

        /// <summary>
        /// 服务区域
        /// </summary>
        public string ServiceArea { get; set; }

        /// <summary>
        /// 服务商地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 纬度值
        /// </summary>
        public string lat { get; set; }

        /// <summary>
        /// 经度值
        /// </summary>
        public string lng { get; set; }

        /// <summary>
        /// 维修员人数
        /// </summary>
        public int RepairManNumber { get; set; }

        /// <summary>
        /// 账号Id
        /// </summary>
        public Guid AccountInfoId { get; set; }

        /// <summary>
        /// 提交时间
        /// </summary>
        public string SubmitTime { get; set; }

        /// <summary>
        /// 审核状态0：待审核， 1：审核通过，2：审核未通过
        /// </summary>
        public int AuditStatus { get; set; }
    }
}
