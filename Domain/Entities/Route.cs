using System.Collections.Generic;

namespace Domain.Entities
{
    public class Route
    {
        public int Id { get; set; }

        public virtual Train Train { get; set; }

        public virtual ICollection<RouteStation> Stations { get; set; }

        public bool IsDeleted { get; set; }
    }
}