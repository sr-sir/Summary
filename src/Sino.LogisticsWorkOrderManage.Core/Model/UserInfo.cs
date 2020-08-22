using System;
using System.Collections.Generic;
using System.Text;
using Sino.Domain.Entities;


namespace Sino.LogisticsWorkOrderManage.Core.Models
{
    public class UserInfo : Entity<Guid>
    {
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        public string IDCard { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 账号Id
        /// </summary>
        public Guid AccountInfoId { get; set; }

        /// <summary>
        /// 身份证正面照片
        /// </summary>
        public string IdCardFrontPicUrl { get; set; }

        /// <summary>
        /// 身份证反面照片
        /// </summary>
        public string IdCardBackPicUrl { get; set; }

        /// <summary>
        /// 提交时间
        /// </summary>
        public string SubmitTime { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>
        public string AuditTime { get; set; }

        /// <summary>
        /// 实名认证审核状态：0：待审核， 1：审核通过，2：审核未通过
        /// </summary>
        public int AuditStatus { get; set; }
    }
}
