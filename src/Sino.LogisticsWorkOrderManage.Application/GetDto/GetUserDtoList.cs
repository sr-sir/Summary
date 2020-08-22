using System;
using System.Collections.Generic;
using System.Text;

namespace Sino.LogisticsWorkOrderManage.Application.GetDto
{
    public class GetUserDtoList
    {
        public List<GetUserDto> UserDtoList { get; set; }
        public int Count { get; set; }
    }

    public class GetUserDto
    {
        /// <summary>
        /// 客户Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }

        /// <summary>
        /// 登陆账户
        /// </summary>
        public string LoginPhone { get; set; }
    }
}
