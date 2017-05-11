using Domain.Entities;

namespace BLL.Abstract
{
    public interface ITicketService
    {
        void GenerateTicket(Ticket ticket);
    }
}