using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRT.Domain.Enums;

namespace TRT.Application.DTOs.ReservationDTOs
{
    public class ReservationDetailDTO
    {
        public string Id { get; set; }
        public string ReferenceNumber { get; set; }
        public string PassengerClass { get; set; }
        public string DestinationStationName { get; set; }
        public string TrainName { get; set; }
        public string ArrivalStationName { get; set; }
        public string DateTime { get; set; }
        public int NoOfPassengers { get; set; }
        public decimal Price { get; set; }
        public string CreatedByUser { get; set; }
        public string Status { get; set; }
    }
}
