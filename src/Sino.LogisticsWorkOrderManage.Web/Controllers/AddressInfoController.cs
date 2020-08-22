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
    public class AddressInfoController : BaseController
    {

        protected IAddressInfoService _AddressInfoService { get; set; }

        public AddressInfoController(IAddressInfoService addressInfoService)
        {
            _AddressInfoService = addressInfoService;
        }


        /// <summary>
        /// 新增客户地址
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("AddAddressInfo")]
        public async Task AddAddressInfo([FromBody]AddAddressInfoRequest request)
        {
            _logger.Info(new LogInfo() { Method = nameof(AddAddressInfo), Argument = new { request }, Description = "新增客户地址" });
            await _AddressInfoService.AddAddressInfoAsync(request);
        }

        /// <summary>
        /// 获取客户地址列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("GetListAsync")]
        public async Task<List<AddressInfo>> GetListAsync([FromQuery]AddressSearchRequest request)
        {
            _logger.Info(new LogInfo() { Method = nameof(GetListAsync), Argument = new { request }, Description = "获取客户地址列表" });
            var userIdClaim = User.Claims.FirstOrDefault(t => t.Type == JwtRegisteredClaimNames.Sid);
            Guid? userId = userIdClaim != null ? (Guid?)Guid.Parse(userIdClaim.Value) : null;
            if (!request.UserId.HasValue)
            {
                request.UserId = userId;
            }
            return await _AddressInfoService.GetListAsync(request);
        }

    }
}