using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using BLL.Abstract;
using Domain.Entities;

namespace BLL.Concrete
{
    public class MailSender : IMailSender
    {
        public async Task SendEmail(string targetMail, Ticket ticket)
        {
            MailAddress from = new MailAddress("oleksandr.shustov@nure.ua", "RAILWAYS PROJECT");
            MailAddress to = new MailAddress("shustov.sanya@gmail.com" /*targetMail*/);
            MailMessage message = new MailMessage(from, to);

            message.Subject = "Your ticket";
            message.Body = "<h2>Please, find your ticket in attach! </br> Best regards, RAILWAYS PROJECT</h2>";
            message.Attachments.Add(new Attachment(@"X:\Ticket.pdf"));
            message.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("oleksandr.shustov@nure.ua", "hefestojasper12"),
                EnableSsl = true
            };

            smtp.Send(message);
        }
    }
}