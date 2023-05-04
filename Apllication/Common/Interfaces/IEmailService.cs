using Application.Common.Models.DTOs;
using Domain.Entities.Users;

namespace Application.Common.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(User user, List<GameDto> recomendedGames);

    string GenerateEmailBody(List<GameDto> recomendedGames);
}
