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
    public class AddressInfoRepository : DapperRepositoryBase<AddressInfo, long>, IAddressInfoRepository
    {
        public AddressInfoRepository(IDapperConfiguration configuration) : base(configuration, true)
        {

        }

        private Tuple<string, DynamicParameters> GetWhere(AddressSearchRequest request, DynamicParameters parameters)
        {
            StringBuilder sbWhere = new StringBuilder();

            if (request.UserId.HasValue)
            {
                sbWhere.Append(" AND UserId = @UserId ");
                parameters.Add("@UserId", request.UserId.Value);
            }

            if (request.Province.IsNotNullAndEmpty())
            {
                sbWhere.Append(" AND Province = @Province ");
                parameters.Add("@Province", request.Province);
            }

            if (request.City.IsNotNullAndEmpty())
            {
                sbWhere.Append(" AND City = @City ");
                parameters.Add("@City", request.City);
            }

            if (request.County.IsNotNullAndEmpty())
            {
                sbWhere.Append(" AND County = @County ");
                parameters.Add("@County", request.County);
            }

            return new Tuple<string, DynamicParameters>(sbWhere.ToString(), parameters);
        }

        public async Task<List<AddressInfo>> GetListAsync(AddressSearchRequest request)
        {
            StringBuilder sbSql = new StringBuilder();
            DynamicParameters parameters = new DynamicParameters();
            sbSql.Append(" SELECT * FROM addressinfo WHERE IsDeleted = 0  ");
            var where = this.GetWhere(request, parameters);
            if (where?.Item1?.Length > 0)
            {
                sbSql.Append(where.Item1);
            }
            //添加排序
            sbSql.Append(" Order By CreationTime DESC ");
            using (ReadConnection)
            {
                var r1 = await ReadConnection.QueryAsync<AddressInfo>(sbSql.ToString(), parameters);
                return r1.ToList();
            }
        }


    }
}
