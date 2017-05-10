using System.Threading.Tasks;
using Domain.Entities;

namespace BLL.Abstract
{
    public interface IRouteStationService
    {
        Task AddStationToRoute(int routeId, RouteStation station);
    }
}