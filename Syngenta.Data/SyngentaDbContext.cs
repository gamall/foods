using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Text;
using System.Threading.Tasks;
using Syngenta.Core;
//using Syngenta.Data.Configurations;

namespace Syngenta.Data
{

    public class SyngentaDbContext : DbContext
    {
        static SyngentaDbContext()
        {
            //dont't do anything            
        }


        public SyngentaDbContext()
            : base(nameOrConnectionString: "SyngentaConnectionString")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Configurations.Add(new OrganisationConfiguration());
            //modelBuilder.Configurations.Add(new UserConfiguration());

            //// Add ASP.NET WebPages SimpleSecurity tables
            //modelBuilder.Configurations.Add(new RoleConfiguration());
            //modelBuilder.Configurations.Add(new MembershipConfiguration());
            //modelBuilder.Configurations.Add(new OAuthMembershipConfiguration());

        }


        public DbSet<User> Users { get; set; }
        public DbSet<UserBook> UserBooks { get; set; }        
        public DbSet<UserBookPage> UserBookPages { get; set; }
        public DbSet<FailedLoginAttempt> FailedLoginAttempt { get; set; }
        public DbSet<UserModificationLog> UserModificationLogs { get; set; }

    }
}
