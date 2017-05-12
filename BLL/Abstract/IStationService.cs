using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace BLL.Abstract
{
    public interface IStationService
    {
        Task<List<Station>> GetAll();

        Task CreateStation(Station station);

        Task DeleteStation(int stationId);

        List<Station> FindByTerm(string term);
    }
}