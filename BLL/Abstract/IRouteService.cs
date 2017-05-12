using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace BLL.Abstract
{
    public interface IRouteService
    {
        Task<List<Route>> GetAll();

        Task DeleteRoute(int id);

        Route CreateRoute();

        Task AddStationToRoute(int routeId, RouteStation station);

        Task<Route> GetById(int id);

        Task ActivateRoute(int routeId);

        Task UpdateRoute(Route route);

        Task AddTrainToRoute(int trainId, int routeId);

        Task RemoveStationFromRoute(int routeId, int routeStationId);

        Task<List<Route>> GetRoutesBetweenStations(string from, string to);
    }
}