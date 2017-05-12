using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using Domain.Repositories;

namespace DAL.Repositories
{
    public class StationRepository : Repository<Station>, IStationRepository
    {
        public StationRepository(ApplicationDbContext context) : base(context)
        {
        }

        public List<Station> FindByTerm(string term)
        {
            return Set.Where(s => s.Name.StartsWith(term)).ToList();
        }
    }
}