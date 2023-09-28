using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using TRT.Domain.Enums;

namespace TRT.Domain.Entities
{
    public class Station
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; }

        [BsonElement("Code")]
        public string Code { get; set; }

        [BsonElement("Elevation")]
        public decimal Elevation { get; set; }

        [BsonElement("Distance")]
        public decimal Distance { get; set; }

        [BsonElement("Line")]
        public Line Line { get; set; }

        [BsonElement("City")]
        public string City { get; set; }
    }
}
