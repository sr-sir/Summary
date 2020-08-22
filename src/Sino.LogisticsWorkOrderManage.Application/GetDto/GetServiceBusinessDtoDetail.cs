using System;
using System.Collections.Generic;
using System.Text;

namespace Sino.LogisticsWorkOrderManage.Application.GetDto
{
    public class GetServiceBusinessDtoDetail
    {
        /// <summary>
        /// 服务商编号
        /// </summary>
        public string ServiceBusinessNo { get; set; }

        /// <summary>
        /// 服务商名称
        /// </summary>
        public string ServiceBusinessName { get; set; }

        /// <summary>
        /// 主营业务
        /// </summary>
        public List<string> MainBusiness { get; set; }

        /// <summary>
        /// 主营产品
        /// </summary>
        public List<string> MainProducts { get; set; }

        /// <summary>
        /// 服务区域
        /// </summary>
        public List<PCA> ServiceArea { get; set; }

        /// <summary>
        /// 服务商地址
        /// </summary>
        public PCA Address { get; set; }

        /// <summary>
        /// 维修人员列表
        /// </summary>
        public List<RepairManInfoS> repairManInfos { get; set; }
    }

    public class RepairManInfoS
    {
        /// <summary>
        /// 维修人员姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string ContactPhoneNumber { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        public string IdNumber { get; set; }

        /// <summary>
        /// 提交服务商时间
        /// </summary>
        public string SubmitServerBusinessTime { get; set; }

        /// <summary>
        /// 服务商审核时间
        /// </summary>
        public string AuditServerBusinessTime { get; set; }

        /// <summary>
        /// 账户是否启用
        /// </summary>
        public bool IsEnable { get; set; }
    }
}
