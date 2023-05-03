using Apllication.Common.Interfaces;
using Domain.Entities.Companies;
using Domain.Entities.Users;
using Domain.Enums;
using System.Security.Claims;

namespace Presentration.API.Services;

public sealed class UserService : IUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserId? UserId { get; init; }

    public CompanyId? CompanyId { get; init; }

    public RoleType RoleType { get; init; }

    public UserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;

        if (_httpContextAccessor.HttpContext is not null)
        {
            var idClaim = _httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier);

            if (idClaim is not null)
            {
                UserId = Guid.TryParse(idClaim.Value, out Guid userId) ? new(userId) : null;
                RoleType = Enum.Parse<RoleType>(_httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.Role)!.Value);

                var companyIdClaim = _httpContextAccessor.HttpContext!.User.FindFirst("CompanyId");

                if (companyIdClaim is not null)
                    CompanyId = Guid.TryParse(companyIdClaim.Value, out Guid companyId) ? new(companyId) : null;
            }
        }
    }
}
