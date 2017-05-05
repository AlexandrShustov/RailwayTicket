using System;

namespace Domain.Entities
{
    public class RouteStation
    {
        public int Id { get; set; }

        public virtual Station Station { get; set; }

        public DateTime DepartureTime { get; set; }

        public DateTime ArriveTime { get; set; }
    }
}