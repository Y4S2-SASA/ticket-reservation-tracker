using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace TRT.Domain.Entities
{
    public class Schedule
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("DepartureStation")]
        public string DepartureStation { get; set; }

        [BsonElement("ArrivalStation")]
        public string ArrivalStation { get; set; }

        [BsonElement("DepartureTime")]
        public DateTime DepartureTime { get; set; }

        [BsonElement("ArrivalTime")]
        public DateTime ArrivalTime { get; set; }
    }
}
