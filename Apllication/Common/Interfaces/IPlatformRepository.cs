using Application.Common.Interfaces.Repositories.Base;
using Domain.Entities.Platforms;

namespace Application.Common.Interface;

public interface IPlatformRepository : IBaseRepository<Platform, PlatformId>
{
}
