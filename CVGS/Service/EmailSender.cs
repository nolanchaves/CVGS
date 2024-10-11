using CVGS.Interfaces;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

public class EmailSender : IEmailSender
{
    public async Task SendEmailAsync(string email, string subject, string message)
    {
        var smtpClient = new SmtpClient("smtp.zoho.com")
        {
            Port = 465,
            Credentials = new NetworkCredential("cvgstestemail@zohomailcloud.ca", "fq4UgyQNj50z\r\n"),
            EnableSsl = true,
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress("cvgstestemail@zohomailcloud.ca"),
            Subject = subject,
            Body = message,
            IsBodyHtml = true,
        };

        mailMessage.To.Add(email);

        await smtpClient.SendMailAsync(mailMessage);
    }
}

