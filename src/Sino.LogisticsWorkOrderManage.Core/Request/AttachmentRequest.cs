using System;
using System.Collections.Generic;
using System.Text;

namespace Sino.LogisticsWorkOrderManage.Core
{
    public class AttachmentItemRequest
    {
        /// <summary>
        /// 附件名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get; set; }

    }
}
