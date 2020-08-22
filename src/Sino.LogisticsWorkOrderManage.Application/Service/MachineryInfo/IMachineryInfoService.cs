using Sino.LogisticsWorkOrderManage.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sino.LogisticsWorkOrderManage.Application
{
    public interface IMachineryInfoService
    {

        Task<long> Add(AddMachineryInfoRequest request);

        /// <summary>
        /// 查询客户机器
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<List<MachineryInfo>> GetListAsync(MachineryInfoSearchRequest request);
    }
}
