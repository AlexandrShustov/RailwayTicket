using System.Collections.Generic;

namespace Domain.Entities
{
    public class Train
    {
        public int Id { get; set; }

        public int Number { get; set; }

        public virtual ICollection<Carriage> Carriage { get; set; }
    }
}