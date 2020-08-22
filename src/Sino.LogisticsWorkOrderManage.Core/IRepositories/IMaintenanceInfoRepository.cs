using Sino.Dependency;
using Sino.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sino.LogisticsWorkOrderManage.Core.IRepositories
{
    public interface IMaintenanceInfoRepository : IRepository<MaintenanceInfo, Guid>, ITransientDependency
    {
        /// <summary>
        /// 添加工单
        /// </summary>
        /// <param name="body">工单对象</param>
        /// <param name="attachments">工单图片</param>
        /// <returns></returns>
        Task<bool> Add(MaintenanceInfo body, List<Attachment> attachments);

        /// <summary>
        /// 工单查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<Tuple<List<MaintenanceInfoResponse>, int>> GetPageListAsync(MaintenanceInfoPageRequest request);
    }
}
