using Application.Common.Interfaces.Repositories.Base;
using Domain.Entities.Platforms;

namespace Application.Common.Interfaces.Repositories;

public interface IPlatformRepository : IBaseRepository<Platform, PlatformId>
{
}
