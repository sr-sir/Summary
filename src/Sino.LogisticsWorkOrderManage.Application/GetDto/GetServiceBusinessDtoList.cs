using System;
using System.Collections.Generic;
using System.Text;

namespace Sino.LogisticsWorkOrderManage.Application.GetDto
{
    public class GetServiceBusinessDtoList
    {
        public List<GetServiceBusinessDto> getServiceBusinessDtos { get; set; }
        public int Count { get; set; }
    }

    public class GetServiceBusinessDto
    {
        /// <summary>
        /// 服务商Id
        /// </summary>
        public string ServiceBusinessId { get; set; }

        /// <summary>
        /// 服务商编号
        /// </summary>
        public string ServiceBusinessNo { get; set; }

        /// <summary>
        /// 服务商名称
        /// </summary>
        public string ServiceBusinessName { get; set; }

        /// <summary>
        /// 服务商类型：1、金牌服务商  2、一般服务商
        /// </summary>
        public int ServiceBusinessType { get; set; }

        /// <summary>
        /// 服务区域
        /// </summary>
        public List<PCA> ServiceArea { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }

        /// <summary>
        /// 主营业务
        /// </summary>
        public List<string> MainBusiness { get; set; }

        /// <summary>
        /// 主营产品
        /// </summary>
        public List<string> MainProducts { get; set; }
    }
}
