using System.Threading.Tasks;
using BLL.Abstract;
using Domain.Entities;
using Domain.Repositories;

namespace BLL.Concrete
{
    public class RouteStationService : IRouteStationService
    {
        private IUnitOfWork _unitOfWork;

        public RouteStationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddStationToRoute(int routeId, RouteStation station)
        {
            var route = await _unitOfWork.RouteRepository.FindByIdAsync(routeId);

            Guard.ArgumentNotNull(route, nameof(route) + " should not be null.");
            Guard.ArgumentNotNull(station, nameof(station) + " should not be null.");

            route.Stations.Add(await CreateRouteStation(station));

            _unitOfWork.RouteRepository.Update(route);

            await _unitOfWork.SaveChangesAsync();
        }

        private async Task<RouteStation> CreateRouteStation(RouteStation routeStation)
        {
            Guard.ArgumentNotNull(routeStation, nameof(routeStation) + "should not be null.");

            routeStation.Station = await _unitOfWork.StationRepository.FindByIdAsync(routeStation.Station.Id);

            _unitOfWork.RouteStationRepository.Add(routeStation);

            await _unitOfWork.SaveChangesAsync();

            return routeStation;
        }
    }
}