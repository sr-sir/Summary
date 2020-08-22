using Sino.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sino.LogisticsWorkOrderManage.Core.Models
{
    public class AccountInfo : Entity<Guid>
    {
        /// <summary>
        /// 账户登录手机号
        /// </summary>
        public string LoginPhone { get; set; }

        /// <summary>
        /// 账户登录密码
        /// </summary>
        public string LoginPassword { get; set; }

        /// <summary>
        /// OpenId
        /// </summary>
        public string OpenId { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string WeChatName { get; set; }

        /// <summary>
        /// 头像Url
        /// </summary>
        public string AvatarUrl { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }

        /// <summary>
        /// 角色信息：0、管理员  1、客户 2、服务商 3、维修员
        /// </summary>
        public int RoleId { get; set; }
    }
}
