using Sino.Dependency;
using Sino.LogisticsWorkOrderManage.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sino.LogisticsWorkOrderManage.Application
{
    public interface IMaintenanceInfoService : ITransientDependency
    {
        Task<Guid> Add(AddMaintenanceInfoRequest request, Guid? createUserId);

        /// <summary>
        /// 查询工单数据（分页）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<BaseResponse<MaintenanceInfoResponse>> GetPageListAsync(MaintenanceInfoPageRequest request);

        /// <summary>
        /// 工单查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<List<MaintenanceInfoResponse>> GetListAsync(MaintenanceInfoSearchRequest request);

        /// <summary>
        /// 获取单个工单数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<MaintenanceInfoResponse> GetById(Guid id);

        /// <summary>
        /// 指派维修员
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<bool> AssignRepairMan(MaintenanceAssignRpRequest request);

        /// <summary>
        /// 服务商取消工单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<bool> SPCancelMaintenance(SPCancelMaintenanceRequest request);

        /// <summary>
        /// 服务商领取工单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<bool> SPReceiveMaintenance(SPReceiveMaintenanceRequest request);

        /// <summary>
        /// 转派操作
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<bool> TransformAssign(TransformAssignRequest request);

        /// <summary>
        /// 受理操作
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<bool> RPReceiveMaintenance(RPReceiveMaintenanceRequest request);
    }
}
