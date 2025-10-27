using Microsoft.AspNetCore.Http;
using MimeKit;
using MimeKit.Text;
using System.Collections.Generic;
using System.Net.Mail;
using MailKit.Security;
using MailKit.Net.Smtp;
using System.Threading.Tasks;
using System;

namespace API.Models.MyHelper
{

    public class MyEmail
    {
        public static async Task<Boolean> SendEmailAsync(MailSettings setting, MailRequest request, List<string> ListFile = null)
        {
            // create message
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(setting.DisplayName, setting.Mail));
            //email.Sender = MailboxAddress.Parse(setting.Mail);
            email.To.Add(MailboxAddress.Parse(request.ToEmail));
            email.Subject = request.Subject;

            var builder = new BodyBuilder();
            builder.HtmlBody = request.Body;

            if (ListFile != null && ListFile.Count > 0)
            {
                for (int i = 0; i < ListFile.Count; i++)
                {
                    builder.Attachments.Add(ListFile[i]);
                }
            }

            email.Body = builder.ToMessageBody();

            // send email
            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect(setting.Host,setting.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(setting.Mail, setting.Password);
            try
            {
                await smtp.SendAsync(email);
                smtp.Disconnect(true);
                return true;
            }
            catch {
                smtp.Disconnect(true);
                return false;
            }
                  
        }
    }
    
    

    public class MailSettings
    {
        public string? Mail { get; set; }        
        public string? DisplayName { get; set; }
        public string? Password { get; set; }
        public string? Host { get; set; }
        public int Port { get; set; }
    }

    public class MailRequest
    {
        public string? ToEmail { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }
        public List<IFormFile> Attachments { get; set; }
    }
}

//Krongnang: mpcurptqzryqmuhj