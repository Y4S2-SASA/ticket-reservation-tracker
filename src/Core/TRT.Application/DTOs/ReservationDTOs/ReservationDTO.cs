using TRT.Domain.Enums;

namespace TRT.Application.DTOs.ReservationDTOs
{
    public class ReservationDTO
    {
        public string Id { get; set; }
        public string ReferenceNumber { get; set; }
        public PassengerClass PassengerClass { get; set; }
        public string DestinationStationId { get; set; }
        public string TrainId { get; set; }
        public string ArrivalStationId { get; set; }
        public DateTime DateTime { get; set; }
        public int NoOfPassengers { get; set; }
        public decimal Price { get; set; }

    }
}
