using Dapper;
using Sino.Dapper;
using Sino.Dapper.Repositories;
using Sino.LogisticsWorkOrderManage.Core;
using Sino.LogisticsWorkOrderManage.Core.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sino.LogisticsWorkOrderManage.Repositories
{
    public class MachineryInfoRepository : DapperRepositoryBase<MachineryInfo, long>, IMachineryInfoRepository
    {
        public MachineryInfoRepository(IDapperConfiguration configuration) : base(configuration, true)
        {

        }

        private Tuple<string, DynamicParameters> GetWhere(MachineryInfoSearchRequest request, DynamicParameters parameters)
        {
            StringBuilder sbWhere = new StringBuilder();

            if (request.UserId.HasValue)
            {
                sbWhere.Append(" AND UserId = @UserId ");
                parameters.Add("@UserId", request.UserId.Value);
            }

            if (request.PurchaseDate.HasValue)
            {
                sbWhere.Append(" AND PurchaseDate = @PurchaseDate ");
                parameters.Add("@PurchaseDate", request.PurchaseDate.Value);
            }

            return new Tuple<string, DynamicParameters>(sbWhere.ToString(), parameters);
        }

        public async Task<List<MachineryInfo>> GetListAsync(MachineryInfoSearchRequest request)
        {
            StringBuilder sbSql = new StringBuilder();
            DynamicParameters parameters = new DynamicParameters();
            sbSql.Append(" SELECT * FROM machineryinfo WHERE IsDeleted = 0  ");
            var where = this.GetWhere(request, parameters);
            if (where?.Item1?.Length > 0)
            {
                sbSql.Append(where.Item1);
            }
            //添加排序
            sbSql.Append(" Order By CreationTime DESC ");
            using (ReadConnection)
            {
                var r1 = await ReadConnection.QueryAsync<MachineryInfo>(sbSql.ToString(), parameters);
                return r1.ToList();
            }
        }
    }
}
