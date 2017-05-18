using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace BLL.Abstract
{
    public interface ITicketService
    {
        void GeneratePdfTicket(Ticket ticket);

        Task CreateTicket(Ticket ticket);

        Task<decimal> CountTicketPrice(int routeId, string stationFrom, string stationTo, int teaCount, bool isNeedLinen);

        Task<List<Ticket>> GetTicketsByRoute(int routeId);
    }
}