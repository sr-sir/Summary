using AutoMapper;
using Newtonsoft.Json;
using Sino.LogisticsWorkOrderManage.Application.GetDto;
using Sino.LogisticsWorkOrderManage.Core;
using Sino.LogisticsWorkOrderManage.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sino.LogisticsWorkOrderManage.Application
{
    public class AutoMapperInitialize
    {
        public static void InitServiceMap(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<AddMaintenanceInfoRequest, MaintenanceInfo>()
                .ForMember(t => t.ReleaseType, a => a.MapFrom(i => (int)i.ReleaseType))
                .ForMember(t => t.MaintenancType, a => a.MapFrom(i => (int)i.MaintenancType));

            cfg.CreateMap<AddAddressInfoRequest, AddressInfo>();

            cfg.CreateMap<AddMachineryInfoRequest, MachineryInfo>();

            //cfg.CreateMap<MaintenanceInfo, MaintenanceInfoResponse>();
            cfg.CreateMap<Tuple<List<MaintenanceInfoResponse>, int>, BaseResponse<MaintenanceInfoResponse>>()
                .ForMember(x => x.TotalCount, x => x.MapFrom(a => a.Item2))
                .ForMember(x => x.Items, x => x.MapFrom(a => a.Item1));

            cfg.CreateMap<ServiceBusiness, GetServiceBusinessDtoDetail>()
                .ForMember(t => t.MainBusiness, a => a.MapFrom(i => JsonConvert.DeserializeObject<List<string>>(i.MainBusiness)))
                .ForMember(t => t.MainProducts, a => a.MapFrom(i => JsonConvert.DeserializeObject<List<string>>(i.MainProducts)))
                .ForMember(t => t.ServiceArea, a => a.MapFrom(i => JsonConvert.DeserializeObject<List<PCA>>(i.ServiceArea)))
                .ForMember(t => t.Address, a => a.MapFrom(i => JsonConvert.DeserializeObject<PCA>(i.Address)));


        }
    }
}
