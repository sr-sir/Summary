using Microsoft.EntityFrameworkCore;
using Sino.LogisticsWorkOrderManage.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sino.LogisticsWorkOrderManage.Repositories.DbContextFile
{
    public class AccountInfoDbContext : DbContext
    {
        public AccountInfoDbContext(DbContextOptions<AccountInfoDbContext> options) : base(options)
        {

        }

        public DbSet<AccountInfo> accountInfo { get; set; }
        public DbSet<RepairManInfo> repairManInfo { get; set; }
        public DbSet<ServiceBusiness> serviceBusiness { get; set; }
        public DbSet<UserInfo> userInfo { get; set; }
    }
}
