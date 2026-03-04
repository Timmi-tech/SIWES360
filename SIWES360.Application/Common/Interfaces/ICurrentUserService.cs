namespace SIWES360.Application.Common.Interfaces;

public interface ICurrentUserService
{
    string? UserId { get; }
    string? UserName { get; }
    string? Role { get; }
    string? DepartmentId { get; }
    string? DepartmentName { get; }
}
