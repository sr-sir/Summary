using AutoMapper;
using Sino.LogisticsWorkOrderManage.Core;
using Sino.LogisticsWorkOrderManage.Core.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sino.LogisticsWorkOrderManage.Application
{
    public class MachineryInfoService : IMachineryInfoService
    {
        protected IMachineryInfoRepository MachineryInfoRep { get; set; }

        public MachineryInfoService(IMachineryInfoRepository machineryInfoRep)
        {
            MachineryInfoRep = machineryInfoRep;
        }



        /// <summary>
        /// 添加客户机器
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public async Task<long> Add(AddMachineryInfoRequest request)
        {
            var info = Mapper.Map<MachineryInfo>(request);
            info.IsDeleted = false;
            return await MachineryInfoRep.InsertAndGetIdAsync(info);
        }

        /// <summary>
        /// 查询客户机器
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<List<MachineryInfo>> GetListAsync(MachineryInfoSearchRequest request)
        {
            return await MachineryInfoRep.GetListAsync(request);
        }
    }
}
