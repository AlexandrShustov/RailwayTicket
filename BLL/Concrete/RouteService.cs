using System.Collections.Generic;
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

        public Task<List<Route>> GetAll()
        {
            return _unitOfWork.RouteRepository.GetAllAsync();
        }

        public async Task DeleteRoute(int id)
        {
            var route = await _unitOfWork.RouteRepository.FindByIdAsync(id);
            route.IsDeleted = true;
            _unitOfWork.RouteRepository.Update(route);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}