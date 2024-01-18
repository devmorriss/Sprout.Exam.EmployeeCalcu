using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Sprout.Exam.DataAccess.Models;
using Sprout.Exam.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sprout.Exam.WebApp.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        public DbSet<Employee> Employees{ get; set; }
        public DbSet<EmployeeType> EmployeeTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Employee>().ToTable("Employee");
            builder.Entity<EmployeeType>().ToTable("EmployeeType");

            //builder.Entity<EmployeeType>().HasData(
            //    new EmployeeType { Id = 1, TypeName = nameof(Sprout.Exam.Common.Enums.EmployeeType.Regular) },
            //    new EmployeeType { Id = 2, TypeName = nameof(Sprout.Exam.Common.Enums.EmployeeType.Contractual) });
        }
    }
}
