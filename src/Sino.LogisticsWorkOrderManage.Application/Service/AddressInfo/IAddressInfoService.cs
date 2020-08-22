using Sino.Dependency;
using Sino.LogisticsWorkOrderManage.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sino.LogisticsWorkOrderManage.Application
{
    public interface IAddressInfoService : ITransientDependency
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<long> AddAddressInfoAsync(AddAddressInfoRequest request);

        /// <summary>
        /// 获取地址列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<List<AddressInfo>> GetListAsync(AddressSearchRequest request);
    }
}
