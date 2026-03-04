using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Serilog;
using SIWES360.Infrastructure.Services;

public class EmailSender(IOptions<EmailConfiguration> config) : IEmailSender
{
    private readonly EmailConfiguration _config = config.Value;

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email address cannot be null or empty", nameof(email));

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_config.SenderUsername, _config.FromEmail));
        message.To.Add(MailboxAddress.Parse(email));
        message.Subject = subject;

        message.Body = new BodyBuilder { HtmlBody = htmlMessage }.ToMessageBody();

        using var client = new SmtpClient();

        try
        {
            await client.ConnectAsync(
                _config.SmtpServer,
                _config.SmtpPort,
                SecureSocketOptions.StartTls);

            await client.AuthenticateAsync(
                _config.FromEmail,
                _config.SmtpPassword);

            await client.SendAsync(message);

            Log.Information("Email successfully sent to {Email} with subject {Subject}", email, subject);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Email sending failed for {Email}", email);
            throw;
        }
        finally
        {
            if (client.IsConnected)
                await client.DisconnectAsync(true);
        }
    }
}