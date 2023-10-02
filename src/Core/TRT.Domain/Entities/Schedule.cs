using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using TRT.Domain.Enums;

namespace TRT.Domain.Entities
{
    public class Schedule
    {
        public Schedule()
        {
            SubStationDetails = new HashSet<SubStationDetail>();
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("TrainId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string TrainId { get; set; }

        [BsonElement("DepartureStationId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string DepartureStationId { get; set; }

        [BsonElement("ArrivalStationId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ArrivalStationId { get; set; }

        [BsonElement("DepartureTime")]
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime DepartureTime { get; set; }

        [BsonElement("ArrivalTime")]
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime ArrivalTime { get; set; }

        [BsonElement("Status")]
        public Status Status { get; set; }

        [BsonElement("SubStationDetails")]
        public ICollection<SubStationDetail> SubStationDetails { get; set; }


    }

    public class SubStationDetail
    {
        [BsonElement("StationId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string StationId { get; set; }

        [BsonElement("ArrivalTime")]
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime ArrivalTime { get; set; }
    }
}
