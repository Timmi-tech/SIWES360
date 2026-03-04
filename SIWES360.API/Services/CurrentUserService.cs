using Microsoft.AspNetCore.Http;
using SIWES360.Application.Common.Interfaces;
using System.Security.Claims;

namespace SIWES360.API.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? UserId => _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    public string? UserName => _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name)?.Value;
    public string? Role => _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value;
    public string? DepartmentId => _httpContextAccessor.HttpContext?.User.FindFirst("departmentId")?.Value;
    public string? DepartmentName => _httpContextAccessor.HttpContext?.User.FindFirst("departmentName")?.Value;

}
