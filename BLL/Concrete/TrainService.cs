using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Abstract;
using Domain.Entities;
using Domain.Repositories;

namespace BLL.Concrete
{
    public class TrainService : ITrainService
    {
        private IUnitOfWork _unitOfWork;

        public TrainService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Train>> GetAll()
        {
            var trains = await _unitOfWork.TrainRepository.GetAllAsync();

            trains.ForEach(t => t.Carriages = t.Carriages.Where(c => !c.IsDeleted).ToList());

            return trains.Where(t => !t.IsDeleted).ToList();
        }

        public async Task DeleteTrain(int trainId)
        {
            var train = await _unitOfWork.TrainRepository.FindByIdAsync(trainId);

            Guard.ArgumentNotNull(train, nameof(train) + "should not be null.");

            train.IsDeleted = true;

            _unitOfWork.TrainRepository.Update(train);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<Train> GetById(int id)
        {
            var train = await _unitOfWork.TrainRepository.FindByIdAsync(id);

            Guard.ArgumentNotNull(train, nameof(train) + "should not be null.");

            return train;
        }

        public Task UpdateTrain(Train train)
        {
            Guard.ArgumentNotNull(train, nameof(train) + "should not be null.");

            _unitOfWork.TrainRepository.Update(train);

            return _unitOfWork.SaveChangesAsync();
        }

        public Train CreateTrain()
        {
            var train = new Train();
            train.Carriages = new List<Carriage>();

            _unitOfWork.TrainRepository.Add(train);

            _unitOfWork.SaveChanges();

            return _unitOfWork.TrainRepository.GetAll().Last();
        }

        public async Task TakePlace(int trainId, int carriageNumber, int placeNumber)
        {
            var train = _unitOfWork.TrainRepository.FindById(trainId);

            Guard.ArgumentNotNull(train, nameof(train) + " should not be null.");

            train.Carriages.First(c => c.Number == carriageNumber)
                 .Places.First(p => p.Number == placeNumber).IsFree = false;

            _unitOfWork.TrainRepository.Update(train);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}