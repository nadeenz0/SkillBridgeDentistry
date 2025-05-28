using CoreLayer.Services;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

public class EmailSender : IEmailSender
{
    private readonly EmailConfiguration _emailConfig;

    public EmailSender(IOptions<EmailConfiguration> options)
    {
        _emailConfig = options.Value;
    }

    public Task SendEmailAsync(Message message)
    {
        var emailMessage = CreateEmailMessage(message);
        return Send(emailMessage);
    }


    private MimeMessage CreateEmailMessage(Message message)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("Your App", _emailConfig.From));
        emailMessage.To.AddRange(message.To.Select(email => MailboxAddress.Parse(email)));
        emailMessage.Subject = message.Subject;

        var builder = new BodyBuilder();
        builder.HtmlBody = message.Content;
        emailMessage.Body = builder.ToMessageBody();

        return emailMessage;
    }

    private async Task Send(MimeMessage mailMessage)
    {
        using var client = new SmtpClient();
        try
        {
            await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password);
            await client.SendAsync(mailMessage);
        }
        catch
        {
            throw;
        }
        finally
        {
            await client.DisconnectAsync(true);
            client.Dispose();
        }
    }
}


