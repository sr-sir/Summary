using AutoMapper;
using Sino.LogisticsWorkOrderManage.Core;
using Sino.LogisticsWorkOrderManage.Core.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sino.LogisticsWorkOrderManage.Application
{
    public class AddressInfoService : IAddressInfoService
    {
        protected IAddressInfoRepository AddressInfoRep { get; set; }

        public AddressInfoService(IAddressInfoRepository addressInfoRep)
        {
            AddressInfoRep = addressInfoRep;
        }

        /// <summary>
        /// 新增地址
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public async Task<long> AddAddressInfoAsync(AddAddressInfoRequest request)
        {
            var info = Mapper.Map<AddressInfo>(request);
            info.IsDeleted = false;
            return await AddressInfoRep.InsertAndGetIdAsync(info);
        }

        /// <summary>
        /// 获取地址列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<List<AddressInfo>> GetListAsync(AddressSearchRequest request)
        {
            return await this.AddressInfoRep.GetListAsync(request);
        }
    }
}
