using Sino.Dependency;
using Sino.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sino.LogisticsWorkOrderManage.Core.IRepositories
{
    public interface IBasicConfigurationRepository : IRepository<BasicConfiguration, int>, ITransientDependency
    {

    }
}
