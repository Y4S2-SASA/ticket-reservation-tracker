using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using TRT.Domain.Enums;
/*
 * File: Train.cs
 * Author:Jayathilake S.M.D.A.R/IT20037338
*/
namespace TRT.Domain.Entities
{
    public class Train
    {
        public Train()
        {
            PassengerClasses = new List<PassengerClass>();
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("TrainName")]
        public string TrainName { get; set; }

        [BsonElement("SeatCapacity")]
        public int SeatCapacity  { get; set; }

        [BsonElement("AvailableDays")]
        public AvailableDays AvailableDays { get; set; }

        [BsonElement("PassengerClasses")]
        public List<PassengerClass> PassengerClasses { get; set; }

        [BsonElement("Status")]
        public Status Status { get; set; }
    }
}
