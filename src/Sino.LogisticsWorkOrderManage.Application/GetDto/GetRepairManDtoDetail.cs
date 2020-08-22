using System;
using System.Collections.Generic;
using System.Text;

namespace Sino.LogisticsWorkOrderManage.Application.GetDto
{
    public class GetRepairManDtoDetail
    {
        /// <summary>
        /// 头像
        /// </summary>
        public string AvatarUrl { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        public string IdNumber { get; set; }

        /// <summary>
        /// 身份证正面照片
        /// </summary>
        public string IdCardFrontPicUrl { get; set; }

        /// <summary>
        /// 身份证反面照片
        /// </summary>
        public string IdCardBackPicUrl { get; set; }

        /// <summary>
        /// 身份审核状态0：待审核， 1：审核通过，2：审核未通过
        /// </summary>
        public int IdentityAuditStatus { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string ContactPhoneNumber { get; set; }

        /// <summary>
        /// 服务商编号
        /// </summary>
        public string ServiceBusinessNo { get; set; }

        /// <summary>
        /// 服务商名称
        /// </summary>
        public string ServiceBusinessName { get; set; }

        /// <summary>
        /// 服务区域
        /// </summary>
        public List<PCA> ServiceAreas { get; set; }

        /// <summary>
        /// 服务商地址
        /// </summary>
        public PCA ServiceBusinessAddress { get; set; }
    }

    public class PCA
    {
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
        public string Area { get; set; }
    }
}
