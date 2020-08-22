using Sino.Dapper;
using Sino.Dapper.Repositories;
using Sino.LogisticsWorkOrderManage.Core;
using Sino.LogisticsWorkOrderManage.Core.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sino.LogisticsWorkOrderManage.Repositories
{
    public class BasicConfigurationRepository: DapperRepositoryBase<BasicConfiguration, int>, IBasicConfigurationRepository
    {
        public BasicConfigurationRepository(IDapperConfiguration configuration) : base(configuration, true)
        {

        }


    }
}
