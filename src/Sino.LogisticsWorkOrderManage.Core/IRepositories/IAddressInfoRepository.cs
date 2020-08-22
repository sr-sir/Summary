using Sino.Dependency;
using Sino.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sino.LogisticsWorkOrderManage.Core.IRepositories
{
    public interface IAddressInfoRepository : IRepository<AddressInfo, long>, ITransientDependency
    {
        /// <summary>
        /// 获取地址列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<List<AddressInfo>> GetListAsync(AddressSearchRequest request);
    }
}
