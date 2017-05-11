using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace BLL.Abstract
{
    public interface ITrainService
    {
        Task<List<Train>> GetAll();

        Task DeleteTrain(int trainId);

        Task<Train> GetById(int id);

        Task UpdateTrain(Train id);

        Train CreateTrain();

        Task TakePlace(int trainId, int carriageNumber, int placeNumber);
    }
}