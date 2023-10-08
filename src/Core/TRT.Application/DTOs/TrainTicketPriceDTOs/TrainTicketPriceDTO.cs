using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using TRT.Domain.Enums;

namespace TRT.Application.DTOs.TrainTicketPriceDTOs
{
    public class TrainTicketPriceDTO
    {
        public string Id { get; set; }
        public PassengerClass PassengerClass { get; set; }
        public decimal Price { get; set; }
    }
}
