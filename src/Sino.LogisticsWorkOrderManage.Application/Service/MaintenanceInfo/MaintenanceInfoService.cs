using AutoMapper;
using Newtonsoft.Json;
using Sino.CommonService;
using Sino.CommonService.HttpClientLibrary;
using Sino.LogisticsWorkOrderManage.Core;
using Sino.LogisticsWorkOrderManage.Core.IRepositories;
using Sino.LogisticsWorkOrderManage.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sino.LogisticsWorkOrderManage.Application
{
    /// <summary>
    /// 工单
    /// </summary>
    public class MaintenanceInfoService : IMaintenanceInfoService
    {
        /// <summary>
        /// 工单仓储
        /// </summary>
        protected IMaintenanceInfoRepository _MaintenanceInfoRep { get; set; }

        protected IClientProvider ClientProvider { get; set; }

        public MaintenanceInfoService(IMaintenanceInfoRepository maintenanceInfoRep, IClientProvider clientProvider)
        {
            _MaintenanceInfoRep = maintenanceInfoRep;
            ClientProvider = clientProvider;
        }


        /// <summary>
        /// 添加工单
        /// </summary>
        /// <param name="request"></param>
        /// <returns>返回工单主键ID</returns>
        public async Task<Guid> Add(AddMaintenanceInfoRequest request, Guid? createUserId)
        {
            DateTime now = DateTime.Now;
            var entity = Mapper.Map<MaintenanceInfo>(request);
            entity.Id = Guid.NewGuid();
            //已经指定服务商
            if (request.RepairId.HasValue)
            {
                entity.MaintenancStatus = (int)EMaintenancStatus.Assigned;
            }
            else
            {
                entity.MaintenancStatus = (int)EMaintenancStatus.UnAssign;
            }
            entity.CreationTime = now;
            entity.CreatorUserId = createUserId;

            string Address = entity.UserProvince + entity.UserCity + entity.UserCounty + entity.UserAddress;
            var res = await ClientProvider.AddressToPosition(new AddressToPositionRequest()
            {
                Address = Address
            });
            if (res.success == false)
            {
                throw new SinoException(ErrorCode.E30003, nameof(ErrorCode.E30003).GetCode());
            }
            var obj = JsonConvert.DeserializeObject<AddressToPositionReply>(res.data.ToString());
            if (obj != null)
            {
                entity.Lat = obj.Latitude.ToString();
                entity.Lng = obj.Longitude.ToString();
            }
            else
            {
                entity.Lat = string.Empty;
                entity.Lng = string.Empty;
            }

            var attachments = request.AttachmentList.Select(t =>
            {
                return new Attachment()
                {
                    Name = t.Name,
                    Path = t.Path,
                    DataId = entity.Id,
                    DataType = (int)EAttachmentDataType.WorkOrder,
                    CreationTime = now,
                    IsDeleted = false,
                    CreatorUserId = createUserId
                };
            })?.ToList();
            var result = await _MaintenanceInfoRep.Add(entity, attachments);
            return entity.Id;
        }



        /// <summary>
        /// 工单分页查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<BaseResponse<MaintenanceInfoResponse>> GetPageListAsync(MaintenanceInfoPageRequest request)
        {
            Tuple<List<MaintenanceInfoResponse>, int> result = await _MaintenanceInfoRep.GetPageListAsync(request);
            result.Item1?.ForEach(t =>
            {
                t.MaintenancTypeName = t.MaintenancType.HasValue ? t.MaintenancType.Value.GetEnumDescription<EMaintenancType>() : string.Empty;
                t.MaintenancStatusName = t.MaintenancStatus.HasValue ? t.MaintenancStatus.Value.GetEnumDescription<EMaintenancStatus>() : string.Empty;
            });
            var retVal = Mapper.Map<BaseResponse<MaintenanceInfoResponse>>(result);
            return retVal;
        }

        /// <summary>
        /// 工单查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<List<MaintenanceInfoResponse>> GetListAsync(MaintenanceInfoSearchRequest request)
        {
            var myRequest = Mapper.Map<MaintenanceInfoPageRequest>(request);
            var result = await this.GetPageListAsync(myRequest);
            return result?.Items;
        }

        public async Task<MaintenanceInfoResponse> GetById(Guid id)
        {
            MaintenanceInfoPageRequest request = new MaintenanceInfoPageRequest();
            request.Id = id;
            var result = await this.GetPageListAsync(request);
            if (result?.Items?.Count > 0)
            {
                return result.Items[0];
            }
            return null;
        }

        /// <summary>
        /// 指派维修员
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<bool> AssignRepairMan(MaintenanceAssignRpRequest request)
        {
            //判断该工单状态是否可以指定维修员
            var maintenanceInfo = await this.GetById(request.Id.Value);
            if (maintenanceInfo == null)
            {
                throw new SinoException(ErrorCode.E30016, nameof(ErrorCode.E30016).GetCode());
            }

            if (maintenanceInfo.MaintenancStatus.Value != (int)EMaintenancStatus.Received)
            {
                var desc = maintenanceInfo.MaintenancStatus.Value.GetEnumDescription<EMaintenancStatus>();
                throw new SinoException(string.Format(ErrorCode.E30017, desc), nameof(ErrorCode.E30017).GetCode());
            }

            //进行指派操作
            MaintenanceInfo maintenance = new MaintenanceInfo();
            maintenance.MaintenancStatus = (int)EMaintenancStatus.AssignRP;
            maintenance.RepairmanId = request.RepairManId;
            maintenance.Id = request.Id.Value;
            maintenance.LastModificationTime = DateTime.Now;
            List<string> updateCols = new List<string>();
            updateCols.Add(nameof(MaintenanceInfo.MaintenancStatus));
            updateCols.Add(nameof(MaintenanceInfo.RepairmanId));
            updateCols.Add(nameof(MaintenanceInfo.LastModificationTime));
            var result = await _MaintenanceInfoRep.UpdateAssignFieldsById(maintenance, updateCols);
            return result > 0;
        }

        /// <summary>
        /// 服务商取消工单
        /// </summary>
        /// <returns></returns>
        public async Task<bool> SPCancelMaintenance(SPCancelMaintenanceRequest request)
        {
            //判断该工单状态是否可以取消
            var maintenanceInfo = await this.GetById(request.Id.Value);
            if (maintenanceInfo == null)
            {
                throw new SinoException(ErrorCode.E30016, nameof(ErrorCode.E30016).GetCode());
            }

            if (maintenanceInfo.MaintenancStatus.Value > (int)EMaintenancStatus.Received)
            {
                var desc = maintenanceInfo.MaintenancStatus.Value.GetEnumDescription<EMaintenancStatus>();
                throw new SinoException(string.Format(ErrorCode.E30018, desc), nameof(ErrorCode.E30018).GetCode());
            }

            //进行取消操作
            MaintenanceInfo maintenance = new MaintenanceInfo();
            maintenance.MaintenancStatus = (int)EMaintenancStatus.Closed;
            maintenance.Id = request.Id.Value;
            maintenance.LastModificationTime = DateTime.Now;
            List<string> updateCols = new List<string>();
            updateCols.Add(nameof(MaintenanceInfo.MaintenancStatus));
            updateCols.Add(nameof(MaintenanceInfo.LastModificationTime));
            var result = await _MaintenanceInfoRep.UpdateAssignFieldsById(maintenance, updateCols);
            return result > 0;
        }

        /// <summary>
        /// 服务商领取工单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<bool> SPReceiveMaintenance(SPReceiveMaintenanceRequest request)
        {
            //判断该工单状态是否领取工单
            var maintenanceInfo = await this.GetById(request.Id.Value);
            if (maintenanceInfo == null)
            {
                throw new SinoException(ErrorCode.E30016, nameof(ErrorCode.E30016).GetCode());
            }
            //在已指派的状态下才可以领取工单
            if (maintenanceInfo.MaintenancStatus.Value != (int)EMaintenancStatus.Assigned)
            {
                var desc = maintenanceInfo.MaintenancStatus.Value.GetEnumDescription<EMaintenancStatus>();
                throw new SinoException(string.Format(ErrorCode.E30019, desc), nameof(ErrorCode.E30019).GetCode());
            }

            //进行取消操作
            MaintenanceInfo maintenance = new MaintenanceInfo();
            maintenance.MaintenancStatus = (int)EMaintenancStatus.Received;
            maintenance.Id = request.Id.Value;
            maintenance.LastModificationTime = DateTime.Now;
            List<string> updateCols = new List<string>();
            updateCols.Add(nameof(MaintenanceInfo.MaintenancStatus));
            updateCols.Add(nameof(MaintenanceInfo.LastModificationTime));
            var result = await _MaintenanceInfoRep.UpdateAssignFieldsById(maintenance, updateCols);
            return result > 0;
        }

        /// <summary>
        /// 转派操作
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<bool> TransformAssign(TransformAssignRequest request)
        {
            //判断该工单状态是否可以转派工单
            var maintenanceInfo = await this.GetById(request.Id.Value);
            if (maintenanceInfo == null)
            {
                throw new SinoException(ErrorCode.E30016, nameof(ErrorCode.E30016).GetCode());
            }

            //在已受理（已被服务商指派维修员）的状态下才可以领取工单
            if (maintenanceInfo.MaintenancStatus.Value != (int)EMaintenancStatus.AssignRP)
            {
                var desc = maintenanceInfo.MaintenancStatus.Value.GetEnumDescription<EMaintenancStatus>();
                throw new SinoException(string.Format(ErrorCode.E30020, desc), nameof(ErrorCode.E30020).GetCode());
            }

            //进行转派操作
            MaintenanceInfo maintenance = new MaintenanceInfo();
            maintenance.Id = request.Id.Value;
            maintenance.RepairmanId = request.NewRepairManId;
            maintenance.LastModificationTime = DateTime.Now;
            List<string> updateCols = new List<string>();
            updateCols.Add(nameof(MaintenanceInfo.RepairmanId));
            updateCols.Add(nameof(MaintenanceInfo.LastModificationTime));
            var result = await _MaintenanceInfoRep.UpdateAssignFieldsById(maintenance, updateCols);
            return result > 0;
        }

        /// <summary>
        /// 维修员受理操作
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<bool> RPReceiveMaintenance(RPReceiveMaintenanceRequest request)
        {
            //判断该工单状态是否可以转派工单
            var maintenanceInfo = await this.GetById(request.Id.Value);
            if (maintenanceInfo == null)
            {
                throw new SinoException(ErrorCode.E30016, nameof(ErrorCode.E30016).GetCode());
            }

            //在已受理（已被服务商指派维修员）的状态下才可以领取工单
            if (maintenanceInfo.MaintenancStatus.Value != (int)EMaintenancStatus.AssignRP)
            {
                var desc = maintenanceInfo.MaintenancStatus.Value.GetEnumDescription<EMaintenancStatus>();
                throw new SinoException(string.Format(ErrorCode.E30021, desc), nameof(ErrorCode.E30021).GetCode());
            }

            //进行转派操作
            MaintenanceInfo maintenance = new MaintenanceInfo();
            maintenance.Id = request.Id.Value;
            maintenance.MaintenancStatus = (int)EMaintenancStatus.Accepting;
            maintenance.LastModificationTime = DateTime.Now;
            List<string> updateCols = new List<string>();
            updateCols.Add(nameof(MaintenanceInfo.MaintenancStatus));
            updateCols.Add(nameof(MaintenanceInfo.LastModificationTime));
            var result = await _MaintenanceInfoRep.UpdateAssignFieldsById(maintenance, updateCols);
            return result > 0;
        }
    }
}
