using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Sino.LogisticsWorkOrderManage.Application;
using Sino.LogisticsWorkOrderManage.Core;
using Sino.Web.Logging;

namespace Sino.LogisticsWorkOrderManage.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaintenanceInfoController : BaseController
    {

        protected IMaintenanceInfoService _MaintenanceInfoService { get; set; }

        public MaintenanceInfoController(IMaintenanceInfoService maintenanceInfoService)
        {
            _MaintenanceInfoService = maintenanceInfoService;
        }

        /// <summary>
        /// 新增工单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Add")]
        public async Task Add([FromBody]AddMaintenanceInfoRequest request)
        {
            _logger.Info(new LogInfo() { Method = nameof(Add), Argument = new { request }, Description = "添加工单" });
            //var result = await IAccountInfoRepositories.GetAccountByIdAsync(body.AccountinfoId);
            ////if (string.IsNullOrEmpty(result.IdCardBackPicUrl))
            ////{
            ////    throw new SinoException(ErrorCode.E150001, nameof(ErrorCode.E150001).GetCode());
            ////}
            ////if (string.IsNullOrEmpty(result.IdCardFrontPicUrl))
            ////{
            ////    throw new SinoException(ErrorCode.E150001, nameof(ErrorCode.E150001).GetCode());
            ////}
            //if (string.IsNullOrEmpty(result.IdCardNumber))
            //{
            //    throw new SinoException(ErrorCode.E150001, nameof(ErrorCode.E150001).GetCode());
            //}
            //if (string.IsNullOrEmpty(result.RealName))
            //{
            //    throw new SinoException(ErrorCode.E150001, nameof(ErrorCode.E150001).GetCode());
            //}
            //if (body.AgriculturalBrand.Length >= 50)
            //{
            //    throw new SinoException(ErrorCode.E100011, nameof(ErrorCode.E100011).GetCode());
            //}
            //if (body.FactoryNumber.Length >= 50)
            //{
            //    throw new SinoException(ErrorCode.E100011, nameof(ErrorCode.E100011).GetCode());
            //}
            //if (!string.IsNullOrEmpty(body.Remarks))
            //{
            //    if (body.Remarks.Length >= 300)
            //    {
            //        throw new SinoException(ErrorCode.E100011, nameof(ErrorCode.E100011).GetCode());
            //    }
            //}

            var userIdClaim = User.Claims.FirstOrDefault(t => t.Type == JwtRegisteredClaimNames.Sid);
            Guid? userId = userIdClaim != null ? (Guid?)Guid.Parse(userIdClaim.Value) : null;
            await _MaintenanceInfoService.Add(request, userId);
        }

        /// <summary>
        /// 查询工单数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("GetListAsync")]
        public async Task<BaseResponse<MaintenanceInfoResponse>> GetListAsync([FromQuery]MaintenanceInfoPageRequest request)
        {
            _logger.Info(new LogInfo() { Method = nameof(GetListAsync), Argument = new { request }, Description = "查询工单数据" });
            return await _MaintenanceInfoService.GetPageListAsync(request);
        }

        /// <summary>
        /// 根据工单id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetById/{id}")]
        public async Task<MaintenanceInfoResponse> GetById(string id)
        {
            _logger.Info(new LogInfo() { Method = nameof(GetListAsync), Argument = new { id }, Description = "根据工单id查询" });
            Guid maintenanceInfoId;
            if (!Guid.TryParse(id, out maintenanceInfoId))
            {
                throw new SinoException(ErrorCode.E10015, nameof(ErrorCode.E10015).GetCode());
            }
            var result = await _MaintenanceInfoService.GetById(maintenanceInfoId);
            return result;
        }


        /// <summary>
        /// 服务商指派维修员
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("AssignRepairMan")]
        public async Task AssignRepairMan(MaintenanceAssignRpRequest request)
        {
            _logger.Info(new LogInfo() { Method = nameof(AssignRepairMan), Argument = new { request }, Description = "服务商指派维修员" });
            await _MaintenanceInfoService.AssignRepairMan(request);
        }


        /// <summary>
        /// 服务商取消工单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("SPCancelMaintenance")]
        public async Task SPCancelMaintenance(SPCancelMaintenanceRequest request)
        {
            _logger.Info(new LogInfo() { Method = nameof(SPCancelMaintenance), Argument = new { request }, Description = "服务商取消工单" });
            await _MaintenanceInfoService.SPCancelMaintenance(request);
        }

        /// <summary>
        /// 服务商领取工单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("SPReceiveMaintenance")]
        public async Task SPReceiveMaintenance(SPReceiveMaintenanceRequest request)
        {
            _logger.Info(new LogInfo() { Method = nameof(SPReceiveMaintenance), Argument = new { request }, Description = "服务商领取工单" });
            await _MaintenanceInfoService.SPReceiveMaintenance(request);
        }

        /// <summary>
        /// 导出工单数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("ExportMaintenances")]
        public async Task<IActionResult> ExportMaintenances(MaintenanceInfoSearchRequest request)
        {
            _logger.Info(new LogInfo()
            {
                Method = nameof(ExportMaintenances),
                Argument = new
                {
                    request
                },
                Description = "导出工单数据"
            });
            //获取库存详情
            var result = await this._MaintenanceInfoService.GetListAsync(request);
            List<string> menus = new List<string>()
                    {
                        "工单单号",
                        "创建时间",
                        "工单类型",
                        "客户名称",
                        "客户地址",
                        "当前状态",
                        "服务商"
                    };
            var stream = ExportWithOneTier(nameof(ExportMaintenances), menus, result, (item, i) =>
            {
                string value = string.Empty;
                switch (i)
                {
                    case 0: value = item.Numbers; break;
                    case 1: value = item.CreationTime.Value.ToString("yyyy-MM-dd HH:mm:ss"); break;
                    case 2: value = item.MaintenancTypeName; break;
                    case 3: value = item.UserName; break;
                    case 4: value = item.UserProvince + item.UserCity + item.UserCounty + item.UserAddress; break;
                    case 5: value = item.MaintenancStatusName; break;
                    case 6: value = item.ServiceBusiness?.ServiceBusinessName; break;
                }
                return value;
            });
            return File(stream, "application/ms-excel;charset=UTF-8");
        }

        /// <summary>
        /// 维修员转派操作
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("TransformAssign")]
        public async Task TransformAssign(TransformAssignRequest request)
        {
            _logger.Info(new LogInfo() { Method = nameof(TransformAssign), Argument = new { request }, Description = "维修员转派操作" });
            await _MaintenanceInfoService.TransformAssign(request);
        }

        /// <summary>
        /// 维修员受理操作
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("RPReceiveMaintenance")]
        public async Task RPReceiveMaintenance(RPReceiveMaintenanceRequest request)
        {
            _logger.Info(new LogInfo() { Method = nameof(RPReceiveMaintenance), Argument = new { request }, Description = "维修员受理操作" });
            await _MaintenanceInfoService.RPReceiveMaintenance(request);
        }
    }
}