using System.Collections.Generic;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IStationRepository : IRepository<Station>
    {
        List<Station> FindByTerm(string term);
    }
}