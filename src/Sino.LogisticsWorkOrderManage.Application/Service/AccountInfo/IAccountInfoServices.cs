using Sino.LogisticsWorkOrderManage.Application.GetDto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sino.LogisticsWorkOrderManage.Application.IServices
{
    public interface IAccountInfoServices
    {
        /// <summary>
        /// 获取客户列表
        /// </summary>
        /// <param name="LoginPhone"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="Skip"></param>
        /// <param name="Take"></param>
        /// <returns></returns>
        Task<GetUserDtoList> GetUserList(string LoginPhone, string StartTime, string EndTime, int? Skip, int? Take);

        /// <summary>
        /// 获取客户详情
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        Task<GetUserDtoDetail> GetUserDetail(string UserId);

        /// <summary>
        /// 获取维修员列表
        /// </summary>
        /// <param name="LoginPhone"></param>
        /// <param name="ServiceBusinessNo"></param>
        /// <param name="ServiceBusinessName"></param>
        /// <param name="AuditStatus"></param>
        /// <param name="Skip"></param>
        /// <param name="Take"></param>
        /// <returns></returns>
        Task<GetRepairManDtoList> GetRepairManList(string LoginPhone, string ServiceBusinessNo, string ServiceBusinessName, int? AuditStatus, int? Skip, int? Take);


        /// <summary>
        /// 获取维修员详情
        /// </summary>
        /// <param name="RepairManId"></param>
        /// <returns></returns>
        Task<GetRepairManDtoDetail> GetRepairManDetail(string RepairManId);

        /// <summary>
        /// 获取服务商列表
        /// </summary>
        /// <param name="ServiceBusinessNo"></param>
        /// <param name="ServiceBusinessName"></param>
        /// <param name="ServiceBusinessType"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="Skip"></param>
        /// <param name="Take"></param>
        /// <returns></returns>
        Task<GetServiceBusinessDtoList> GetServiceBusinessList(string ServiceBusinessNo, string ServiceBusinessName, int? ServiceBusinessType, string StartTime, string EndTime, int? Skip, int? Take);

        /// <summary>
        /// 获取服务商详情
        /// </summary>
        /// <param name="ServiceBusinessId"></param>
        /// <returns></returns>
        Task<GetServiceBusinessDtoDetail> GetServiceBusinessDetail(string ServiceBusinessId);


    }
}
