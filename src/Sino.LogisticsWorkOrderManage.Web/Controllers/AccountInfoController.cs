using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sino.LogisticsWorkOrderManage.Application.GetDto;
using Sino.LogisticsWorkOrderManage.Application.IServices;

namespace Sino.LogisticsWorkOrderManage.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountInfoController : ControllerBase
    {
        private readonly IAccountInfoServices _accountInfoServices;
        public AccountInfoController(IAccountInfoServices accountInfoServices)
        {
            _accountInfoServices = accountInfoServices;
        }

        #region 客户管理
        /// <summary>
        /// 获取客户列表
        /// </summary>
        /// <param name="LoginPhone"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="Skip"></param>
        /// <param name="Take"></param>
        /// <returns></returns>
        [HttpGet("GetUserList")]
        public async Task<GetUserDtoList> GetUserList(string LoginPhone, string StartTime, string EndTime, int? Skip, int? Take)
        {
            var output = await _accountInfoServices.GetUserList(LoginPhone, StartTime, EndTime, Skip, Take);
            return output;
        }

        /// <summary>
        /// 获取客户详情
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [HttpGet("GetUserDetail")]
        public async Task<GetUserDtoDetail> GetUserDetail(string UserId)
        {
            var output = await _accountInfoServices.GetUserDetail(UserId);
            return output;
        }

        #endregion

        #region 维修员管理
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
        [HttpGet("GetRepairManList")]
        public async Task<GetRepairManDtoList> GetRepairManList(string LoginPhone, string ServiceBusinessNo, string ServiceBusinessName, int? AuditStatus, int? Skip, int? Take)
        {
            var output = await _accountInfoServices.GetRepairManList(LoginPhone, ServiceBusinessNo, ServiceBusinessName, AuditStatus, Skip, Take);
            return output;
        }

        /// <summary>
        /// 获取维修员详情
        /// </summary>
        /// <param name="RepairManId"></param>
        /// <returns></returns>
        [HttpGet("GetRepairManDetail")]
        public async Task<GetRepairManDtoDetail> GetRepairManDetail(string RepairManId)
        {
            var output = await _accountInfoServices.GetRepairManDetail(RepairManId);
            return output;
        }

        #endregion

        #region 服务商管理

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
        [HttpGet("GetServiceBusinessList")]
        public async Task<GetServiceBusinessDtoList> GetServiceBusinessList(string ServiceBusinessNo, string ServiceBusinessName, int ServiceBusinessType, string StartTime, string EndTime, int? Skip, int? Take)
        {
            var output = await _accountInfoServices.GetServiceBusinessList(ServiceBusinessNo, ServiceBusinessName, ServiceBusinessType, StartTime, EndTime, Skip, Take);
            return output;
        }

        /// <summary>
        /// 获取服务商详情
        /// </summary>
        /// <param name="ServiceBusinessId"></param>
        /// <returns></returns>
        [HttpGet("GetServiceBusinessDetail")]
        public async Task<GetServiceBusinessDtoDetail> GetServiceBusinessDetail(string ServiceBusinessId)
        {
            var output = await _accountInfoServices.GetServiceBusinessDetail(ServiceBusinessId);
            return output;
        }
        #endregion
    }
}