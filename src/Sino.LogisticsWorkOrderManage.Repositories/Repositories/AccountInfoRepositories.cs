using Dapper;
using Sino.Dapper;
using Sino.Dapper.Repositories;
using Sino.LogisticsWorkOrderManage.Core.IRepositories;
using Sino.LogisticsWorkOrderManage.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sino.LogisticsWorkOrderManage.Repositories.Repositories
{
    public class AccountInfoRepositories : DapperRepositoryBase<UserInfo, Guid>, IAccountInfoRepositories
    {
        public AccountInfoRepositories(IDapperConfiguration configuration) : base(configuration)
        {
        }
    }
}
