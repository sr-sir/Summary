using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sino.LogisticsWorkOrderManage.Application;
using Sino.LogisticsWorkOrderManage.Core;
using Sino.Web.Logging;

namespace Sino.LogisticsWorkOrderManage.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MachineryInfoController : BaseController
    {
        protected IMachineryInfoService _MachineryInfoService { get; set; }

        public MachineryInfoController(IMachineryInfoService machineryInfoService)
        {
            _MachineryInfoService = machineryInfoService;
        }

        /// <summary>
        /// 添加客户机器
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("AddMachineryInfo")]
        public async Task AddMachineryInfo([FromBody]AddMachineryInfoRequest request)
        {
            _logger.Info(new LogInfo() { Method = nameof(AddMachineryInfo), Argument = new { request }, Description = "添加机器" });
            await _MachineryInfoService.Add(request);
        }

        /// <summary>
        /// 获取客户机器
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("GetListAsync")]
        public async Task<List<MachineryInfo>> GetListAsync([FromQuery]MachineryInfoSearchRequest request)
        {
            _logger.Info(new LogInfo() { Method = nameof(GetListAsync), Argument = new { request }, Description = "获取客户地址列表" });
            var userIdClaim = User.Claims.FirstOrDefault(t => t.Type == JwtRegisteredClaimNames.Sid);
            Guid? userId = userIdClaim != null ? (Guid?)Guid.Parse(userIdClaim.Value) : null;
            if (!request.UserId.HasValue)
            {
                request.UserId = userId;
            }
            return await _MachineryInfoService.GetListAsync(request);
        }

    }
}