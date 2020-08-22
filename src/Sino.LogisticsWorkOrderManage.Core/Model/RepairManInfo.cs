using Sino.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sino.LogisticsWorkOrderManage.Core.Models
{
    public class RepairManInfo : Entity<Guid>
    {
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }


        /// <summary>
        /// 身份证号
        /// </summary>
        public string IdNumber { get; set; }



        /// <summary>
        /// 账号Id
        /// </summary>
        public Guid AccountInfoId { get; set; }

        /// <summary>
        /// 服务商Id
        /// </summary>
        public Guid ServiceBusinessId { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string ContactPhoneNumber { get; set; }

        /// <summary>
        /// 身份证正面照片
        /// </summary>
        public string IdCardFrontPicUrl { get; set; }

        /// <summary>
        /// 身份证反面照片
        /// </summary>
        public string IdCardBackPicUrl { get; set; }

        /// <summary>
        /// 提交服务商时间
        /// </summary>
        public string SubmitServerBusinessTime { get; set; }

        /// <summary>
        /// 服务商审核时间
        /// </summary>
        public string AuditServerBusinessTime { get; set; }

        /// <summary>
        /// 服务商审核状态0：待审核， 1：审核通过，2：审核未通过
        /// </summary>
        public int ServerBusinessAuditStatus { get; set; }

        /// <summary>
        /// 身份提交时间
        /// </summary>
        public string SubmitIdentityTime { get; set; }

        /// <summary>
        /// 身份审核时间
        /// </summary>
        public string AuditIdentityTime { get; set; }

        /// <summary>
        /// 身份审核状态0：待审核， 1：审核通过，2：审核未通过
        /// </summary>
        public int IdentityAuditStatus { get; set; }

        /// <summary>
        /// 账号是否启用
        /// </summary>
        public bool IsEnable { get; set; }
    }
}
