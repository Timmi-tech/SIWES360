using Microsoft.AspNetCore.Identity;
using SIWES360.Domain.Common;
using SIWES360.Domain.Enums;

namespace SIWES360.Domain.Entities.User
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public UserRole Role { get; set; }
    }
}