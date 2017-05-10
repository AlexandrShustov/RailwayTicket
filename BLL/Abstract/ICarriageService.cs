using System.Threading.Tasks;
using Domain.Entities;

namespace BLL.Abstract
{
    public interface ICarriageService
    {
        Task<Carriage> GetById(int carriageId);

        Task DeleteCarriage(int carriageId, int trainId);

        Task CreateCarriage(Carriage carriage);

        Task AddCarriageToTrain(Carriage carriage, int trainId);
    }
}