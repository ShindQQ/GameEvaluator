using Application.Common.Interfaces;
using Application.Common.Models.DTOs;
using Domain.Entities.Users;
using MailBodyPack;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Presentration.API.Options;

namespace Presentration.API.Services;

public class EmailService : IEmailService
{
    private readonly EmailOptions _emailOptions;

    public EmailService(IOptions<EmailOptions> emailOptions)
    {
        _emailOptions = emailOptions.Value;
    }

    public async Task SendEmailAsync(User user, List<GameDto> recomendedGames)
    {
        var emailMessage = new MimeMessage();
        var bodyBuilder = new BodyBuilder();

        emailMessage.From.Add(new MailboxAddress("GameEvaluator", _emailOptions.Username));

        using var client = new SmtpClient();
        await client.ConnectAsync(_emailOptions.Host, 587, SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(_emailOptions.Username, _emailOptions.Password);

        emailMessage.To.Add(new MailboxAddress("To", user.Email));
        emailMessage.Subject = "Recomended games";
        bodyBuilder.HtmlBody = GenerateEmailBody(recomendedGames);
        emailMessage.Body = bodyBuilder.ToMessageBody();

        await client.SendAsync(emailMessage);
        await client.DisconnectAsync(true);
    }

    public string GenerateEmailBody(List<GameDto> recomendedGames)
    {
        return MailBody.CreateBody()
            .Title("Here is your list of recomended games!")
            .Paragraph("")
            .UnorderedList(recomendedGames
                .Select(game =>
                    MailBody.CreateBlock()
                    .Text($"Name: {game.Name}")))
            .ToString();
    }
}
