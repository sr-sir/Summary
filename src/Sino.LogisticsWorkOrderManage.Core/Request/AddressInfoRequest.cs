using System;
using System.Collections.Generic;
using System.Text;

namespace Sino.LogisticsWorkOrderManage.Core
{
    /// <summary>
    /// 添加地址信息请求对象
    /// </summary>
    public class AddAddressInfoRequest
    {
        /// <summary>
        /// 用户外键
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        ///省
        /// </summary>

        public string Province { get; set; }

        /// <summary>
        /// 市
        /// </summary>

        public string City { get; set; }

        /// <summary>
        /// 区
        /// </summary>

        public string County { get; set; }

        /// <summary>
        /// 详细地址
        /// </summary>
        public string Address { get; set; }
    }


    /// <summary>
    /// 地址查询
    /// </summary>
    public class AddressSearchRequest
    {
        /// <summary>
        /// 用户外键
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        ///省
        /// </summary>

        public string Province { get; set; }

        /// <summary>
        /// 市
        /// </summary>

        public string City { get; set; }

        /// <summary>
        /// 区
        /// </summary>

        public string County { get; set; }

    }
}
