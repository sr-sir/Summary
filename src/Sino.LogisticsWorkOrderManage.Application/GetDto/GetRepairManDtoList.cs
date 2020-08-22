using System;
using System.Collections.Generic;
using System.Text;

namespace Sino.LogisticsWorkOrderManage.Application.GetDto
{
    public class GetRepairManDtoList
    {
        public List<GetRepairManDto> getRepairManDtos { get; set; }

        public int Count { get; set; }
    }

    public class GetRepairManDto
    {
        /// <summary>
        /// 维修员Id
        /// </summary>
        public string RepairManId { get; set; }

        /// <summary>
        /// 服务商编号
        /// </summary>
        public string ServiceBusinessNo { get; set; }

        /// <summary>
        /// 服务商名称
        /// </summary>
        public string ServiceBusinessName { get; set; }

        /// <summary>
        /// 登陆账号
        /// </summary>
        public string LoginPhone { get; set; }

        /// <summary>
        /// 审核状态0：待审核， 1：审核通过，2：审核未通过
        /// </summary>
        public int AuditStatus { get; set; }

        /// <summary>
        /// 是否是金牌服务商
        /// </summary>
        public bool IsGoldServiceBusiness { get; set; }
    }
}
