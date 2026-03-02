using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SIWES360.Application.Common.Interfaces;
using SIWES360.Domain.Entities.Departments;
using SIWES360.Domain.Entities.User;

namespace SIWES360.Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<User>, IApplicationDbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Department> Departments { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }
    }   
}
