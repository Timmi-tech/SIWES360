using Microsoft.EntityFrameworkCore;
using SIWES360.Domain.Entities.User;

namespace  SIWES360.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        public DbSet<User> Users { get; set; }
        Task<int> SaveChangesAsync(CancellationToken ct);
    }
}