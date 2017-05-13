using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using BLL.Abstract;
using Domain.Entities;

namespace BLL.Concrete
{
    public class MailSender : IMailSender
    {
        public async Task SendEmail(string targetMail)
        {
            MailAddress from = new MailAddress("railwaystask@gmail.com", "RAILWAYS PROJECT");
            MailAddress to = new MailAddress(targetMail);

            using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
            using (MailMessage message = new MailMessage(from, to))
            {
                message.Subject = "Your ticket";
                message.Body = "<h2>Please, find your ticket in attach! </br> Best regards, RAILWAYS PROJECT</h2>";
                message.Attachments.Add(new Attachment(AppDomain.CurrentDomain.BaseDirectory + @"\Ticket.pdf"));
                message.IsBodyHtml = true;


                smtp.Credentials = new NetworkCredential("railwaystask@gmail.com", "railwaysfinal777");
                smtp.EnableSsl = true;

                smtp.Send(message);
            }
        }
    }
}