using Sino.Dependency;
using Sino.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sino.LogisticsWorkOrderManage.Core.IRepositories
{
    public interface IMachineryInfoRepository : IRepository<MachineryInfo, long>, ITransientDependency
    {
        /// <summary>
        /// 查询客户机器
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<List<MachineryInfo>> GetListAsync(MachineryInfoSearchRequest request);
    }
}
