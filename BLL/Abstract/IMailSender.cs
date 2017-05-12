using System.Threading.Tasks;
using Domain.Entities;

namespace BLL.Abstract
{
    public interface IMailSender
    {
        Task SendEmail(string targetMail, Ticket ticket);
    }
}