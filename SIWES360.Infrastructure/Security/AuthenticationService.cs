using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using SIWES360.Application.Common.Interfaces;
using SIWES360.Application.Common.Models;
using SIWES360.Application.Features.Authentication;
using SIWES360.Domain.Entities.User;
using SIWES360.Domain.Enums;
using Serilog;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;

namespace SIWES360.Infrastructure.Security
{
    public sealed class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<User> _userManager;
        private readonly IOptions<JwtConfiguration> _configuration;
        private readonly JwtConfiguration _jwtConfiguration;
        private readonly IApplicationDbContext _context;

        public AuthenticationService(UserManager<User> userManager, IOptions<JwtConfiguration> configuration, IApplicationDbContext context)
        {
            _userManager = userManager;
            _configuration = configuration;
            _jwtConfiguration = _configuration.Value;
            _context = context;
        }
        public async Task<Result> RegisterUserAsync(string firstname, string lastname, string matricNo, string email, string password, UserRole role, Guid departmentId, CancellationToken ct)
        {
            var dept = await _context.Departments.AsNoTracking().FirstOrDefaultAsync(d => d.Id == departmentId, cancellationToken: ct);

            if (dept is null)
            {
                Log.Warning("Registration attempt failed: Department with ID {DepartmentId} not found.", departmentId);
                return Result.Failure(Error.NotFound("Department", departmentId.ToString()));
            }

            var exists = await _userManager.Users.AnyAsync(u => u.MatricNumber == matricNo || u.Email == email, cancellationToken: ct);
            if (exists)
            {
                Log.Warning("Registration attempt failed: User with Matric Number {MatricNo} or Email {Email} already exists.", matricNo, email);
                return Result.Failure(Error.Validation("DuplicateUser", "A user with this Matric Number or Email already exists."));
            }
            User user = new()
            {
                FirstName = firstname,
                LastName = lastname,
                MatricNumber = matricNo,
                Email = email,
                UserName = email,
                Role = UserRole.Student,
                DepartmentId = departmentId,
            };
            var identityResult = await _userManager.CreateAsync(user, password);

            return identityResult.Succeeded
                ? Result.Success("Registration successful! Your account has been created.")
                : Result.Failure(Error.Validation("UserCreationFailed", string.Join(", ", identityResult.Errors.Select(e => e.Description))));
        }
        public async Task<Result<TokenResponse>> LoginAsync(string identifier, string password, CancellationToken ct)
        {

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.MatricNumber == identifier || u.Email == identifier || u.UserName == identifier, ct);
            if (user == null)
            {
                Log.Warning("Login attempt failed: User with identifier {Identifier} not found.", identifier);
                return Result<TokenResponse>.Failure(Error.NotFound("User", identifier));
            }
            if (!await _userManager.CheckPasswordAsync(user, password))
            {
                Log.Warning("Login attempt failed: Incorrect password for user with identifier {Identifier}.", identifier);
                return Result<TokenResponse>.Failure(Error.Validation("InvalidCredentials", "Invalid email or password."));
            }
            return await CreateTokenAsync(user, populateExp: true);
        }
        private async Task<Result<TokenResponse>> CreateTokenAsync(User user, bool populateExp)
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims(user);
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

            string refreshToken = GenerateRefreshToken();

            user.RefreshToken = HashRefreshToken(refreshToken);

            if (populateExp)
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await _userManager.UpdateAsync(user);

            string accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            var response = new TokenResponse(accessToken, refreshToken);

            return Result<TokenResponse>.Success(response);
        }
        private SigningCredentials GetSigningCredentials()
        {
            byte[] key = Encoding.UTF8.GetBytes(_jwtConfiguration.Secret);
            SymmetricSecurityKey secret = new(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }
        private async Task<List<Claim>> GetClaims(User user)
        {
            var claims = new List<Claim>
    {
        new(ClaimTypes.NameIdentifier, user.Id),
        new(ClaimTypes.Name, user.UserName ?? string.Empty),
        new(ClaimTypes.Role, user.Role.ToString()),
    };

            if (user.DepartmentId is Guid deptId)
            {
                var deptName = await _context.Departments
                    .AsNoTracking()
                    .Where(d => d.Id == deptId)
                    .Select(d => d.Name)
                    .FirstOrDefaultAsync();

                if (!string.IsNullOrEmpty(deptName))
                {
                    claims.Add(new("departmentId", deptId.ToString()));
                    claims.Add(new("departmentName", deptName));
                }
            }
            return claims;
        }
        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            JwtSecurityToken tokenOptions = new(
                issuer: _jwtConfiguration.ValidIssuer,
                audience: _jwtConfiguration.ValidAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_jwtConfiguration.ExpiresMinutes)),
                signingCredentials: signingCredentials
            );
            return tokenOptions;
        }
        private static string GenerateRefreshToken()
        {
            byte[] randomNumber = new byte[32];
            using RandomNumberGenerator rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
        private static string HashRefreshToken(string refreshToken)
        {
            byte[] tokenBytes = Encoding.UTF8.GetBytes(refreshToken);
            byte[] hashBytes = SHA256.HashData(tokenBytes);
            return Convert.ToBase64String(hashBytes);
        }
        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            TokenValidationParameters tokenValidationParameters = new()
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.Secret!)),
                ValidateLifetime = false,
                ValidIssuer = _jwtConfiguration.ValidIssuer,
                ValidAudience = _jwtConfiguration.ValidAudience,
                ClockSkew = TimeSpan.FromMinutes(5)
            };

            JwtSecurityTokenHandler tokenHandler = new();
            ClaimsPrincipal principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token Format");
            }
            return principal;
        }
        public async Task<Result<TokenResponse>> RefreshTokenAsync(string accessToken, string refreshToken, CancellationToken ct)
        {
            ClaimsPrincipal principal = GetPrincipalFromExpiredToken(accessToken);
            string? userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Result<TokenResponse>.Failure(Error.Validation("InvalidToken", "Invalid token - user ID not found"));

            User? user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return Result<TokenResponse>.Failure(Error.NotFound("User", userId));

            string refreshTokenHash = HashRefreshToken(refreshToken);
            if (user.RefreshToken != refreshTokenHash)
            {
                Log.Warning($"Invalid refresh token attempt for user: {userId}");
                return Result<TokenResponse>.Failure(Error.Validation("InvalidRefreshToken", "Invalid refresh token"));
            }

            if (user.RefreshTokenExpiryTime == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                Log.Warning("Expired refresh token attempt for user: {UserId}", userId);
                return Result<TokenResponse>.Failure(Error.Validation("ExpiredRefreshToken", "Refresh token expired"));
            }

            return await CreateTokenAsync(user, populateExp: true);
        }
        public async Task<Result> RevokeAsync(string userId, CancellationToken ct)
        {
            User? user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return Result.Failure(Error.NotFound("User", userId));

            user.RefreshToken = null!;
            user.RefreshTokenExpiryTime = null;
            await _userManager.UpdateAsync(user);

            return Result.Success("Refresh token revoked successfully");
        }


    }
}
