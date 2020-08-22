using System;
using System.Collections.Generic;
using System.Text;

namespace Sino.LogisticsWorkOrderManage.Application.GetDto
{
    public class GetUserDtoDetail
    {
        /// <summary>
        /// 头像Url
        /// </summary>
        public string AvatarUrl { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        public string IDCard { get; set; }

        /// <summary>
        /// 身份证正面照片
        /// </summary>
        public string IdCardFrontPicUrl { get; set; }

        /// <summary>
        /// 身份证反面照片
        /// </summary>
        public string IdCardBackPicUrl { get; set; }

        /// <summary>
        /// 实名认证审核状态：0：待审核， 1：审核通过，2：审核未通过
        /// </summary>
        public int AuditStatus { get; set; }
    }
}
