using System.Net.Mail;
using System.Net;

namespace FileUploadService.Services
{
    public interface IEmail_Service
    {
        public Task SendAsync(string? sender_name, List<string> recipients, List<string> cc, string subject, string content);
    }
    public class Email_Service : IEmail_Service
    {
        private readonly string username_Credential = "outsource.backend@gmail.com";
        private readonly string password_Credential = "baldcekitfdoldzw";
        public Email_Service() { }
        public async Task SendAsync(string? sender_name, List<string> recipients, List<string> cc, string subject, string content)
        {
            string from = username_Credential;
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(username_Credential, password_Credential),
                EnableSsl = true,
                UseDefaultCredentials = false
            };

            MailMessage mailMessage = new MailMessage();

            foreach (string rp in recipients)
            {
                mailMessage.To.Add(rp);
            }

            foreach (string c in cc)
            {
                mailMessage.CC.Add(c);
            }

            mailMessage.Subject = subject;
            mailMessage.Body = content;
            mailMessage.From = new MailAddress(from, sender_name);
            mailMessage.IsBodyHtml = true;

            smtpClient.SendAsync(mailMessage, null);
        }
    }
}
