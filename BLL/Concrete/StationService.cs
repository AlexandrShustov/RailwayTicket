using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Abstract;
using Domain.Entities;
using Domain.Repositories;

namespace BLL.Concrete
{
    public class StationService : IStationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public StationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<List<Station>> GetAll()
        {
            return _unitOfWork.StationRepository.GetAllAsync();
        }

        public Task CreateStation(Station station)
        {
            Guard.ArgumentNotNull(station, nameof(station) + "should be not null.");
            _unitOfWork.StationRepository.Add(station);

            return _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteStation(int stationId)
        {
            var station = await _unitOfWork.StationRepository.FindByIdAsync(stationId);

            Guard.ArgumentNotNull(station, nameof(station) + "should not be null.");

            station.IsDeleted = true;

            _unitOfWork.StationRepository.Update(station);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}