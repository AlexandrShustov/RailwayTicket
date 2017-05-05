using System.Collections.Generic;

namespace Domain.Entities
{
    public class Carriage
    {
        public int Id { get; set; }

        public virtual ICollection<Place> Places { get; set; }
    }
}