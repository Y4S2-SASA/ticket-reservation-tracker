using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using TRT.Domain.Enums;

namespace TRT.Domain.Entities
{
    public class TrainTicketPrice
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("PassengerClass")]
        public PassengerClass PassengerClass { get; set; }

        [BsonElement("Price")]
        public decimal Price { get; set; }
    }
}
