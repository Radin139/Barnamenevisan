using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Logging;

namespace Barnamenevisan.Core.Senders;

public class EmailSender(ILogger<EmailSender> logger)
{
    public bool SendEmail(string to, string title, string subject, string body)
    {
        try
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            string baseEmail = "radinprogram@gmail.com";
            mail.From = new MailAddress(baseEmail, title);
            mail.To.Add(to);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            
            SmtpServer.Port = 587;
            SmtpServer.EnableSsl = true;

            string appid = "qawp yydq fgwx ijik";
            SmtpServer.Credentials = new NetworkCredential(baseEmail, appid);
            SmtpServer.Send(mail);
            
            return true;
        }
        catch (Exception exception)
        {
            logger.LogError($"Email Error: {exception.Message}");
            return false;
        }
    }
}