using Apllication.Common.Models.DTOs;
using Domain.Entities.Users;

namespace Apllication.Common.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(User user, List<GameDto> recomendedGames);

    string GenerateEmailBody(List<GameDto> recomendedGames);
}
