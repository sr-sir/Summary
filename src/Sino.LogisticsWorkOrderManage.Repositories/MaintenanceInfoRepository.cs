using Dapper;
using Sino.Dapper;
using Sino.Dapper.Repositories;
using Sino.LogisticsWorkOrderManage.Core;
using Sino.LogisticsWorkOrderManage.Core.IRepositories;
using Sino.LogisticsWorkOrderManage.Core.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sino.LogisticsWorkOrderManage.Repositories
{
    public class MaintenanceInfoRepository : DapperRepositoryBase<MaintenanceInfo, Guid>, IMaintenanceInfoRepository, IBulkAddOrUpdate
    {
        public MaintenanceInfoRepository(IDapperConfiguration configuration) : base(configuration, true)
        {

        }

        /// <summary>
        /// 添加维修单
        /// </summary>
        /// <returns></returns>
        public async Task<bool> Add(MaintenanceInfo body, List<Attachment> attachments)
        {
            using (WriteConnection)
            {
                var conn = WriteConnection;
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                //开启事务
                var trans = conn.BeginTransaction();
                try
                {
                    DateTime now = DateTime.Now;
                    //获取最大编号
                    string maxCode = await this.GetMaxCode(nameof(MaintenanceInfo.Numbers), conn, trans, t =>
                    {
                        string dealMaxCode = string.Empty;
                        string currentDate = now.ToString("yyMMdd");
                        if (!string.IsNullOrEmpty(t) && t.Length >= 10 && t.Substring(1, 6) == currentDate)
                        {
                            int len = t.Length - 7;
                            string order = t.Substring(7, len).TrimStart('0');
                            int intOrder = int.Parse(order);
                            if (intOrder < 999)
                            {
                                dealMaxCode = $"1{currentDate}{(intOrder + 1).ToString().PadLeft(3, '0')}";
                            }
                            else
                            {
                                dealMaxCode = $"1{currentDate}{(intOrder + 1).ToString()}";
                            }
                        }
                        else
                        {
                            dealMaxCode = $"1{currentDate}001";
                        }
                        return dealMaxCode;
                    });
                    body.Numbers = maxCode;

                    if (attachments?.Count > 0)
                    {
                        object result = await this.BulkAddAsync3<Attachment>(attachments, conn: conn, trans: trans);

                        if (!(result.GetType() == typeof(int) && Convert.ToInt32(result) > 0))
                        {
                            throw new SinoException(ErrorCode.E30001, nameof(ErrorCode.E30001).GetCode());
                        }
                    }

                    int affectedRows = await this.AddAsync(body);
                    bool isSuccess = affectedRows > 0;
                    if (!isSuccess)
                    {
                        throw new SinoException(ErrorCode.E30001, nameof(ErrorCode.E30001).GetCode());
                    }
                    //提交事务
                    trans.Commit();
                    return isSuccess;
                }
                catch (Exception ex)
                {
                    //回滚事务
                    trans.Rollback();
                    throw ex;
                }
                finally
                {
                    trans.Dispose();
                    conn.Close();
                }
            }

        }


        private Tuple<string, DynamicParameters> GetWhere(MaintenanceInfoSearchRequest request, DynamicParameters parameters)
        {
            StringBuilder sbWhere = new StringBuilder();

            //判断是否根据Id查询
            if (request.Id.HasValue)
            {
                sbWhere.Append(" AND m.Id = @Id ");
                parameters.Add("@Id", request.Id.Value);
            }

            if (request.RoleId.HasValue)
            {
                var roleId = request.RoleId;
                //不同角色获取不同tab菜单的工单数据
                if (request.MaintenancType.HasValue)
                {
                    var maintenancType = request.MaintenancType.Value;
                    List<int> maintenancStatusList = new List<int>();
                    //客户
                    if (roleId == 1)
                    {
                        //获取自己下的工单
                        if (request.UserId.HasValue)
                        {
                            sbWhere.Append(" AND  m.UserId = @UserId ");
                            parameters.Add("@UserId", request.UserId.Value);
                        }
                        #region 工单状态
                        //三种状态
                        //待受理
                        if (maintenancType == (int)EMaintenancTabType.ToBeAccepted)
                        {
                            maintenancStatusList.Add((int)EMaintenancStatus.UnAssign);
                            maintenancStatusList.Add((int)EMaintenancStatus.Assigned);
                            maintenancStatusList.Add((int)EMaintenancStatus.Received);
                            sbWhere.Append(" AND  m.MaintenancStatus IN (@MaintenancStatus) ");
                            parameters.Add("@MaintenancStatus", maintenancStatusList);
                        }
                        //受理中
                        else if (maintenancType == (int)EMaintenancTabType.Acceptance)
                        {
                            maintenancStatusList.Add((int)EMaintenancStatus.AssignRP);
                            maintenancStatusList.Add((int)EMaintenancStatus.Accepting);
                            sbWhere.Append(" AND  m.MaintenancStatus IN (@MaintenancStatus) ");
                            parameters.Add("@MaintenancStatus", maintenancStatusList);
                        }
                        //已关闭
                        else if (maintenancType == (int)EMaintenancTabType.Closed)
                        {
                            maintenancStatusList.Add((int)EMaintenancStatus.Complete);
                            maintenancStatusList.Add((int)EMaintenancStatus.Closed);
                            sbWhere.Append(" AND  m.MaintenancStatus IN (@MaintenancStatus) ");
                            parameters.Add("@MaintenancStatus", maintenancStatusList);
                        }
                        else
                        {
                            maintenancStatusList.Add((int)EMaintenancStatus.UnAssign);
                            maintenancStatusList.Add((int)EMaintenancStatus.Assigned);
                            maintenancStatusList.Add((int)EMaintenancStatus.Received);
                            maintenancStatusList.Add((int)EMaintenancStatus.AssignRP);
                            maintenancStatusList.Add((int)EMaintenancStatus.Accepting);
                            maintenancStatusList.Add((int)EMaintenancStatus.Complete);
                            maintenancStatusList.Add((int)EMaintenancStatus.Closed);
                            sbWhere.Append(" AND  m.MaintenancStatus IN (@MaintenancStatus) ");
                            parameters.Add("@MaintenancStatus", maintenancStatusList);
                        }
                        #endregion
                    }
                    //服务商
                    else if (roleId == 2)
                    {
                        //指定为本服务商的工单
                        if (request.RepairId.HasValue)
                        {
                            sbWhere.Append(" AND  m.RepairId = @RepairId ");
                            parameters.Add("@RepairId", request.RepairId.Value);
                        }
                        #region 工单状态
                        //待受理
                        if (maintenancType == (int)EMaintenancTabType.ToBeAccepted)
                        {
                            maintenancStatusList.Add((int)EMaintenancStatus.Assigned);
                            maintenancStatusList.Add((int)EMaintenancStatus.Received);
                            sbWhere.Append(" AND  m.MaintenancStatus IN (@MaintenancStatus) ");
                            parameters.Add("@MaintenancStatus", maintenancStatusList);
                        }
                        //已受理
                        else if (maintenancType == (int)EMaintenancTabType.Accepted)
                        {
                            maintenancStatusList.Add((int)EMaintenancStatus.AssignRP);
                            sbWhere.Append(" AND  m.MaintenancStatus IN (@MaintenancStatus) ");
                            parameters.Add("@MaintenancStatus", maintenancStatusList);
                        }
                        //受理中
                        else if (maintenancType == (int)EMaintenancTabType.Acceptance)
                        {
                            maintenancStatusList.Add((int)EMaintenancStatus.Accepting);
                            sbWhere.Append(" AND  m.MaintenancStatus IN (@MaintenancStatus) ");
                            parameters.Add("@MaintenancStatus", maintenancStatusList);
                        }
                        //已关闭
                        else if (maintenancType == (int)EMaintenancTabType.Closed)
                        {
                            maintenancStatusList.Add((int)EMaintenancStatus.Complete);
                            maintenancStatusList.Add((int)EMaintenancStatus.Closed);
                            sbWhere.Append(" AND  m.MaintenancStatus IN (@MaintenancStatus) ");
                            parameters.Add("@MaintenancStatus", maintenancStatusList);
                        }
                        else
                        {
                            maintenancStatusList.Add((int)EMaintenancStatus.Assigned);
                            maintenancStatusList.Add((int)EMaintenancStatus.Received);
                            maintenancStatusList.Add((int)EMaintenancStatus.AssignRP);
                            maintenancStatusList.Add((int)EMaintenancStatus.Accepting);
                            maintenancStatusList.Add((int)EMaintenancStatus.Complete);
                            maintenancStatusList.Add((int)EMaintenancStatus.Closed);
                            sbWhere.Append(" AND  m.MaintenancStatus IN (@MaintenancStatus) ");
                            parameters.Add("@MaintenancStatus", maintenancStatusList);
                        }
                        #endregion
                    }
                    //维修员
                    else if (roleId == 3)
                    {
                        //指派给某一个维修员的工单
                        if (request.RepairManId.HasValue)
                        {
                            sbWhere.Append(" AND  m.RepairmanId = @RepairmanId ");
                            parameters.Add("@RepairmanId", request.RepairManId.Value);
                        }
                        #region 工单状态
                        //已受理
                        if (maintenancType == (int)EMaintenancTabType.Accepted)
                        {
                            maintenancStatusList.Add((int)EMaintenancStatus.AssignRP);
                            sbWhere.Append(" AND  m.MaintenancStatus IN (@MaintenancStatus) ");
                            parameters.Add("@MaintenancStatus", maintenancStatusList);
                        }
                        //受理中
                        else if (maintenancType == (int)EMaintenancTabType.Acceptance)
                        {
                            maintenancStatusList.Add((int)EMaintenancStatus.Accepting);
                            sbWhere.Append(" AND  m.MaintenancStatus IN (@MaintenancStatus) ");
                            parameters.Add("@MaintenancStatus", maintenancStatusList);
                        }
                        //已关闭
                        else if (maintenancType == (int)EMaintenancTabType.Closed)
                        {
                            maintenancStatusList.Add((int)EMaintenancStatus.Complete);
                            maintenancStatusList.Add((int)EMaintenancStatus.Closed);
                            sbWhere.Append(" AND  m.MaintenancStatus IN (@MaintenancStatus) ");
                            parameters.Add("@MaintenancStatus", maintenancStatusList);
                        }
                        else
                        {
                            maintenancStatusList.Add((int)EMaintenancStatus.AssignRP);
                            maintenancStatusList.Add((int)EMaintenancStatus.Accepting);
                            maintenancStatusList.Add((int)EMaintenancStatus.Complete);
                            maintenancStatusList.Add((int)EMaintenancStatus.Closed);
                            sbWhere.Append(" AND  m.MaintenancStatus IN (@MaintenancStatus) ");
                            parameters.Add("@MaintenancStatus", maintenancStatusList);
                        }
                        #endregion
                    }
                }
            }

            return new Tuple<string, DynamicParameters>(sbWhere.ToString(), parameters);
        }

        public async Task<Tuple<List<MaintenanceInfoResponse>, int>> GetPageListAsync(MaintenanceInfoPageRequest request)
        {
            StringBuilder sbSql = new StringBuilder();
            StringBuilder sbCount = new StringBuilder();
            DynamicParameters parameters = new DynamicParameters();
            sbSql.Append(@" SELECT * FROM maintenanceinfo m 
                            LEFT JOIN servicebusiness s ON m.RepairId = s.Id
                            LEFT JOIN attachment a ON m.Id = a.DataId AND a.IsDeleted = 0 AND a.DataType = 1 
                            WHERE 1=1 ");
            sbCount.Append(" SELECT COUNT(*) FROM maintenanceinfo m WHERE 1=1 ");
            var where = this.GetWhere(request, parameters);
            if (where?.Item1?.Length > 0)
            {
                sbSql.Append(where.Item1);
                sbCount.Append(where.Item1);
            }
            //添加排序
            sbSql.Append(" Order By m.CreationTime DESC ");
            if (request.Count.HasValue && request.Count.Value > 0)
            {
                sbSql.Append(" LIMIT @Skip,@Count ");
                parameters.Add("@Skip", request.Skip.Value);
                parameters.Add("@Count", request.Count.Value);
            }
            using (ReadConnection)
            {
                var lookup = new Dictionary<Guid, MaintenanceInfoResponse>();
                var query = ReadConnection.Query<MaintenanceInfoResponse, ServiceBusiness, Attachment, List<MaintenanceInfoResponse>>(sbSql.ToString(), (a, c, b) =>
               {
                   if (!lookup.Keys.Contains(a.Id))
                   {
                       if (a != null && c != null)
                       {
                           a.ServiceBusiness = c;
                       }
                       lookup.Add(a.Id, a);
                       if (lookup[a.Id].Attachments == null)
                       {
                           lookup[a.Id].Attachments = new List<Attachment>();
                       }
                       lookup[a.Id].Attachments.Add(b);
                   }
                   else
                   {
                       lookup[a.Id].Attachments.Add(b);
                   }
                   return lookup.Values.ToList();
               }, parameters);


                int r2 = 0;
                if (request.Count.HasValue && request.Count.Value > 0)
                {
                    r2 = await ReadConnection.QueryFirstAsync<int>(sbCount.ToString(), parameters);
                }
                return new Tuple<List<MaintenanceInfoResponse>, int>(lookup.Values.ToList(), r2);
            }
        }

        // public async Task<> MaintenanceInfoRequest
    }
}
