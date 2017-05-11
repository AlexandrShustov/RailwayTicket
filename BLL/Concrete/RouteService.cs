using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Abstract;
using DAL.Repositories;
using Domain.Entities;
using Domain.Repositories;

namespace BLL.Concrete
{
    public class RouteService : IRouteService
    {
        private IUnitOfWork _unitOfWork;

        public RouteService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Route>> GetAll()
        {
            var routes = await _unitOfWork.RouteRepository.GetAllAsync();
            routes = routes.Where(r => !r.IsDeleted).ToList();

            foreach (var route in routes)
            {
                route.Stations = route.Stations.Where(s => !s.IsDeleted).ToList();

                if (route.Train.IsDeleted)
                {
                    route.IsDeleted = true;
                }

                route.Train.Carriages = route.Train.Carriages.Where(c => !c.IsDeleted).ToList();
            }

            return routes;
        }

        public async Task DeleteRoute(int id)
        {
            var route = await _unitOfWork.RouteRepository.FindByIdAsync(id);
            Guard.ArgumentNotNull(route, nameof(route) + "should not be null.");
            route.IsDeleted = true;
            _unitOfWork.RouteRepository.Update(route);
            await _unitOfWork.SaveChangesAsync();
        }

        public Route CreateRoute()
        {
            var route = new Route();
            route.IsDeleted = true;

            _unitOfWork.RouteRepository.Add(route);

            _unitOfWork.SaveChanges();

            return _unitOfWork.RouteRepository.GetAll().Last();
        }

        public async Task AddStationToRoute(int routeId, RouteStation station)
        {
            var route = await _unitOfWork.RouteRepository.FindByIdAsync(routeId);

            Guard.ArgumentNotNull(route, nameof(route) + " should not be null.");
            Guard.ArgumentNotNull(station, nameof(station) + " should not be null.");

            route.Stations.Add(station);

            _unitOfWork.RouteRepository.Update(route);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<Route> GetById(int id)
        {
            var route = await _unitOfWork.RouteRepository.FindByIdAsync(id);

            route.Stations = route.Stations.Where(s => !s.IsDeleted).ToList();


            if (route.Train != null)
            {
                if (route.Train.IsDeleted)
                {
                    route.IsDeleted = true;
                }

                route.Train.Carriages = route.Train.Carriages.Where(c => !c.IsDeleted).ToList();
            }

            Guard.ArgumentNotNull(route, nameof(route) + " should not be null.");

            return route;
        }

        public async Task ActivateRoute(int routeId)
        {
            var route = await _unitOfWork.RouteRepository.FindByIdAsync(routeId);

            Guard.ArgumentNotNull(route, nameof(route) + "should not be null.");

            route.IsDeleted = false;

            _unitOfWork.RouteRepository.Update(route);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateRoute(Route route)
        {
            Guard.ArgumentNotNull(route, nameof(route) + " should not be null.");

            _unitOfWork.RouteRepository.Update(route);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task AddTrainToRoute(int trainId, int routeId)
        {
            var train = await _unitOfWork.TrainRepository.FindByIdAsync(trainId);
            var route = await _unitOfWork.RouteRepository.FindByIdAsync(routeId);

            Guard.ArgumentNotNull(train, nameof(train) + " should not be null.");
            Guard.ArgumentNotNull(route, nameof(route) + " should not be null.");

            route.Train = train;

            _unitOfWork.RouteRepository.Update(route);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task RemoveStationFromRoute(int routeId, int routeStationId)
        {
            var route = await _unitOfWork.RouteRepository.FindByIdAsync(routeId);
            var routeStation = await _unitOfWork.RouteStationRepository.FindByIdAsync(routeStationId);

            Guard.ArgumentNotNull(route, nameof(route) + " should not be null.");
            Guard.ArgumentNotNull(routeStation, nameof(routeStation) + " should not be null.");

            route.Stations.First(rs => rs.Id == routeStation.Id).IsDeleted = true;

            _unitOfWork.RouteRepository.Update(route);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}