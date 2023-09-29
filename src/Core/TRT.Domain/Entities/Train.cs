using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TRT.Domain.Entities
{
    public class Train
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("TrainName")]
        public string TrainName { get; set; }



        [BsonElement("Schedules")]
        public List<Schedule> Schedules { get; set; }

        [BsonElement("IsActive")]
        public bool IsActive { get; set; }
    }
}
