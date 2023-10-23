using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using TRT.Domain.Enums;
/*
 * File: Reservation.cs
 * Author:Bartholomeusz S.V /IT20274702
*/
namespace TRT.Domain.Entities
{
    public class Reservation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [BsonElement("PassengerClass")]
        public PassengerClass PassengerClass { get; set; }

        [BsonElement("DestinationStationId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string  DestinationStationId { get; set; }

        [BsonElement("TrainId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string TrainId { get; set; }

        [BsonElement("ArrivalStationId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ArrivalStationId { get; set; }

        [BsonElement("DateTime")]
        public DateTime DateTime { get; set; }

        [BsonElement("NoOfPassengers")]
        public int NoOfPassengers { get; set; }

        [BsonElement("Price")]
        public decimal Price { get; set; }

        [BsonElement("Status")]
        public Status Status { get; set; }

        [BsonElement("CreatedUserNIC")]
        public string CreatedUserNIC { get; set; }


    }
}