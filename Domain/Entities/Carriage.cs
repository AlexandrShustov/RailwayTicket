using System.Collections.Generic;
using Domain.Enumerations;

namespace Domain.Entities
{
    public class Carriage
    {
        public int Id { get; set; }

        public virtual ICollection<Place> Places { get; set; }

        public int Number { get; set; }

        public CarriageType CarriageType { get; set; }

        public bool IsDeleted { get; set; }
    }
}