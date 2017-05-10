using System.Linq;
using System.Threading.Tasks;
using BLL.Abstract;
using Domain.Entities;
using Domain.Repositories;

namespace BLL.Concrete
{
    public class CarriageService : ICarriageService
    {
        private IUnitOfWork _unitOfWork;
        private ITrainService _trainService;

        public CarriageService(IUnitOfWork unitOfWork, ITrainService trainService)
        {
            _unitOfWork = unitOfWork;
            _trainService = trainService;
        }

        public async Task<Carriage> GetById(int carriageId)
        {
            var carriage = await _unitOfWork.CarriageRepository.FindByIdAsync(carriageId);

            return carriage.IsDeleted ? null: carriage;
        }

        public async Task DeleteCarriage(int carriageId, int trainId)
        {
            var carriage = await _unitOfWork.CarriageRepository.FindByIdAsync(carriageId);

            Guard.ArgumentNotNull(carriage, nameof(carriage) + "should not be null.");

            carriage.IsDeleted = true;

            _unitOfWork.CarriageRepository.Update(carriage);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task CreateCarriage(Carriage carriage)
        {
            Guard.ArgumentNotNull(carriage, nameof(carriage) + "should not be null.");

            _unitOfWork.CarriageRepository.Add(carriage);

             await _unitOfWork.SaveChangesAsync();
        }

        public async Task AddCarriageToTrain(Carriage carriage, int trainId)
        {
            var train = _unitOfWork.TrainRepository.FindById(trainId);

            Guard.ArgumentNotNull(train, nameof(train) + "should not be null.");
            Guard.ArgumentNotNull(carriage, nameof(train) + "should not be null.");

            train.Carriages.Add(carriage);

            _unitOfWork.TrainRepository.Update(train);

            _unitOfWork.SaveChanges();
        }
    }
}