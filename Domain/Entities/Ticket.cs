using System;

namespace Domain.Entities
{
    public class Ticket
    {
        public int Id { get; set; }

        public int TrainNumber { get; set; }

        public DateTime DepartureTime { get; set; }

        public DateTime ArriveTime { get; set; }

        public string DepartureStationName { get; set; }

        public string ArriveStationName { get; set; }

        public decimal Price { get; set; }

        public string PassangerName { get; set; }

        public int CarriageNumber { get; set; }

        public int PlaceNumber { get; set; }
    }
}