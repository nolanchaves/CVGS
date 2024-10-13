using MimeKit;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using MailKit;
using MailKit.Net.Smtp;

public class EmailService
{
    private const string SmtpHost = "smtp-relay.brevo.com";
    private const int SmtpPort = 587;
    private const string SmtpUsername = "7537d1002@smtp-brevo.com";
    private const string SmtpPassword = "8ZdtLqjUPvfaS07G";
    private const string SenderEmail = "verify@cvgs.com";

    public async Task SendEmailAsync(string recipientEmail, string subject, string body)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("verify@cvgs.com", SenderEmail));
        message.To.Add(new MailboxAddress("", recipientEmail));
        message.Subject = subject;

        message.Body = new TextPart("html")
        {
            Text = body
        };

        using (var client = new MailKit.Net.Smtp.SmtpClient())
        {
            await client.ConnectAsync(SmtpHost, SmtpPort, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(SmtpUsername, SmtpPassword);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}

