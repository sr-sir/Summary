using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NLog.Fluent;
using Sino.LogisticsWorkOrderManage.Application.GetDto;
using Sino.LogisticsWorkOrderManage.Application.IServices;
using Sino.LogisticsWorkOrderManage.Core;
using Sino.LogisticsWorkOrderManage.Core.IRepositories;
using Sino.LogisticsWorkOrderManage.Repositories.DbContextFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sino.LogisticsWorkOrderManage.Application.Services
{
    public class AccountInfoServices : IAccountInfoServices
    {
        private readonly IAccountInfoRepositories _accountInfoRepositories;
        private readonly AccountInfoDbContext _accountInfoDbContext;
        public AccountInfoServices(IAccountInfoRepositories accountInfoRepositories, AccountInfoDbContext accountInfoDbContext)
        {
            _accountInfoRepositories = accountInfoRepositories;
            _accountInfoDbContext = accountInfoDbContext;
        }


        /// <summary>
        /// 获取客户列表
        /// </summary>
        /// <param name="LoginPhone"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="Skip"></param>
        /// <param name="Take"></param>
        /// <returns></returns>
        public async Task<GetUserDtoList> GetUserList(string LoginPhone, string StartTime, string EndTime, int? Skip, int? Take)
        {
            GetUserDtoList output = new GetUserDtoList();
            var res = _accountInfoDbContext.accountInfo
                .Where(o => o.RoleId == 1 &&
                (LoginPhone == null ? true : o.LoginPhone.Contains(LoginPhone))
                && (StartTime == null ? true : Convert.ToInt32(o.CreateTime) >= Convert.ToInt32(StartTime))
                && (EndTime == null ? true : Convert.ToInt32(o.CreateTime) <= Convert.ToInt32(EndTime)));
            var dat = await res.Join(_accountInfoDbContext.userInfo, s => s.Id, j => j.AccountInfoId, (s, j) => new GetUserDto
            {
                UserId = j.Id.ToString(),
                CreateTime = s.CreateTime,
                LoginPhone = s.LoginPhone
            }).Skip(Skip == null ? 0 : Convert.ToInt16(Skip)).Take(Take == null ? 10 : Convert.ToInt16(Take)).ToListAsync();
            var count = await res.Join(_accountInfoDbContext.userInfo, s => s.Id, j => j.AccountInfoId, (s, j) => new GetUserDto
            {
                UserId = j.Id.ToString(),
                CreateTime = s.CreateTime,
                LoginPhone = s.LoginPhone
            }).CountAsync();
            output.UserDtoList = dat;
            output.Count = count;
            return output;
        }


        /// <summary>
        /// 获取客户详情
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public async Task<GetUserDtoDetail> GetUserDetail(string UserId)
        {
            var output = await Queryable.Join(_accountInfoDbContext.userInfo.Where(o => o.Id == new Guid(UserId)), _accountInfoDbContext.accountInfo, u => u.AccountInfoId, a => a.Id, (u, a) => new GetUserDtoDetail
            {
                AvatarUrl = a.AvatarUrl,
                RealName = u.RealName,
                IDCard = u.IDCard,
                IdCardFrontPicUrl = u.IdCardFrontPicUrl,
                IdCardBackPicUrl = u.IdCardBackPicUrl,
                AuditStatus = u.AuditStatus
            }).FirstOrDefaultAsync();
            return output;
        }

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
        public async Task<GetRepairManDtoList> GetRepairManList(string LoginPhone, string ServiceBusinessNo, string ServiceBusinessName, int? AuditStatus, int? Skip, int? Take)
        {
            GetRepairManDtoList getRepairManDtoList = new GetRepairManDtoList();
            var account = _accountInfoDbContext.accountInfo.Where(o => o.RoleId == (int)EAccountRole.RepairMan
                && (LoginPhone == null ? true : o.LoginPhone.Contains(LoginPhone)));

            var services = _accountInfoDbContext.serviceBusiness.Where(
                o => (ServiceBusinessNo == null ? true : o.ServiceBusinessNo.ToString().Contains(ServiceBusinessNo))
                && (ServiceBusinessName == null ? true : o.ServiceBusinessName.Contains(ServiceBusinessName)));
            var repairman = _accountInfoDbContext.repairManInfo.Where(o => (AuditStatus == null ? true : o.ServerBusinessAuditStatus == AuditStatus));

            var res = await repairman.Join(account, r => r.AccountInfoId, a => a.Id, (r, a) => new { r, a })
                .Join(services, t => t.r.ServiceBusinessId, b => b.Id, (t, b) => new GetRepairManDto
                {
                    RepairManId = t.r.Id.ToString(),
                    ServiceBusinessNo = b.ServiceBusinessNo.ToString("D6"),
                    ServiceBusinessName = b.ServiceBusinessName,
                    LoginPhone = t.a.LoginPhone,
                    AuditStatus = t.r.ServerBusinessAuditStatus,
                    IsGoldServiceBusiness = b.ServiceBusinessType == 1 ? true : false
                }).Skip(Skip == null ? 0 : Convert.ToInt16(Skip)).Take(Take == null ? 10 : Convert.ToInt16(Take)).ToListAsync();

            var count = await repairman.Join(account, r => r.AccountInfoId, a => a.Id, (r, a) => new { r, a })
                .Join(services, t => t.r.ServiceBusinessId, b => b.Id, (t, b) => new GetRepairManDto
                {
                }).CountAsync();
            getRepairManDtoList.getRepairManDtos = res;
            getRepairManDtoList.Count = count;

            return getRepairManDtoList;
        }

        /// <summary>
        /// 获取维修员详情
        /// </summary>
        /// <param name="RepairManId"></param>
        /// <returns></returns>
        public async Task<GetRepairManDtoDetail> GetRepairManDetail(string RepairManId)
        {
            var account = _accountInfoDbContext.accountInfo;
            var services = _accountInfoDbContext.serviceBusiness;
            var repairman = _accountInfoDbContext.repairManInfo.Where(o => o.Id == new Guid(RepairManId));
            var output = await repairman.Join(account, r => r.AccountInfoId, a => a.Id, (r, a) => new { r, a })
                .Join(services, t => t.r.ServiceBusinessId, s => s.Id, (t, s) => new GetRepairManDtoDetail
                {
                    AvatarUrl = t.a.AvatarUrl,
                    RealName = t.r.RealName,
                    IdNumber = t.r.IdNumber,
                    IdCardFrontPicUrl = t.r.IdCardFrontPicUrl,
                    IdCardBackPicUrl = t.r.IdCardBackPicUrl,
                    IdentityAuditStatus = t.r.IdentityAuditStatus,
                    ContactPhoneNumber = t.r.ContactPhoneNumber,
                    ServiceBusinessNo = s.ServiceBusinessNo.ToString("D6"),
                    ServiceBusinessName = s.ServiceBusinessName,
                    ServiceAreas = JsonConvert.DeserializeObject<List<PCA>>(s.ServiceArea),
                    ServiceBusinessAddress = JsonConvert.DeserializeObject<PCA>(s.Address)
                }).FirstOrDefaultAsync();
            return output;
        }

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
        public async Task<GetServiceBusinessDtoList> GetServiceBusinessList(string ServiceBusinessNo, string ServiceBusinessName, int? ServiceBusinessType, string StartTime, string EndTime, int? Skip, int? Take)
        {
            GetServiceBusinessDtoList getServiceBusinessDtoList = new GetServiceBusinessDtoList();
            var account = _accountInfoDbContext.accountInfo.Where(o => (StartTime == null ? true : Convert.ToInt32(o.CreateTime) >= Convert.ToInt32(StartTime))
            && (EndTime == null ? true : Convert.ToInt32(o.CreateTime) <= Convert.ToInt32(EndTime)));
            var servicesBussiness = _accountInfoDbContext.serviceBusiness.Where(o => (ServiceBusinessNo == null ? true : o.ServiceBusinessNo.ToString().Contains(ServiceBusinessNo))
            && (ServiceBusinessType == null ? true : o.ServiceBusinessType == ServiceBusinessType));

            var res = await account.Join(servicesBussiness, a => a.Id, s => s.AccountInfoId, (a, s) => new GetServiceBusinessDto
            {
                ServiceBusinessId = s.Id.ToString(),
                ServiceBusinessNo = s.ServiceBusinessNo.ToString("D6"),
                ServiceBusinessName = s.ServiceBusinessName,
                ServiceBusinessType = s.ServiceBusinessType,
                ServiceArea = JsonConvert.DeserializeObject<List<PCA>>(s.ServiceArea),
                CreateTime = a.CreateTime,
                MainBusiness = JsonConvert.DeserializeObject<List<string>>(s.MainBusiness),
                MainProducts = JsonConvert.DeserializeObject<List<string>>(s.MainProducts)
            }).Skip(Skip == null ? 0 : Convert.ToInt16(Skip)).Take(Take == null ? 10 : Convert.ToInt16(Take)).ToListAsync();

            var count = await account.Join(servicesBussiness, a => a.Id, s => s.AccountInfoId, (a, s) => new GetServiceBusinessDto
            {
            }).CountAsync();
            getServiceBusinessDtoList.getServiceBusinessDtos = res;
            getServiceBusinessDtoList.Count = count;
            return getServiceBusinessDtoList;
        }

        /// <summary>
        /// 获取服务商详情
        /// </summary>
        /// <param name="ServiceBusinessId"></param>
        /// <returns></returns>
        public async Task<GetServiceBusinessDtoDetail> GetServiceBusinessDetail(string ServiceBusinessId)
        {
            GetServiceBusinessDtoDetail getServiceBusinessDtoDetail = new GetServiceBusinessDtoDetail();
            var servicebusiness = await _accountInfoDbContext.serviceBusiness.Where(o => o.Id == new Guid(ServiceBusinessId)).FirstOrDefaultAsync();
            var repairmanlist = await _accountInfoDbContext.repairManInfo.Where(o => o.ServiceBusinessId == new Guid(ServiceBusinessId)).ToListAsync();
            getServiceBusinessDtoDetail = Mapper.Map<GetServiceBusinessDtoDetail>(servicebusiness);
            getServiceBusinessDtoDetail.repairManInfos = Mapper.Map<List<RepairManInfoS>>(repairmanlist);
            return getServiceBusinessDtoDetail;
        }
    }
}
