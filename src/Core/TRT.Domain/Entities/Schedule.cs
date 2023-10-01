using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace TRT.Domain.Entities
{
    public class Schedule
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("DepartureStationId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string DepartureStationId { get; set; }


        [BsonElement("ArrivalStationId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ArrivalStationId { get; set; }

        [BsonElement("DepartureTime")]
        public DateTime DepartureTime { get; set; }

        [BsonElement("ArrivalTime")]
        public DateTime ArrivalTime { get; set; }
    }
}
