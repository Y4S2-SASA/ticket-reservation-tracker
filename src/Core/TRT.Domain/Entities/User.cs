using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using TRT.Domain.Enums;
/*
 * File: User.cs
 * Author: Dunusinghe A.V/IT20025526
*/
namespace TRT.Domain.Entities
{
    public class User 
    {
        [BsonId]
        public string NIC { get; set; }

        [BsonElement("FirstName")]
        public string FirstName { get; set; }

        [BsonElement("LastName")]
        public  string LastName { get; set; }

        [BsonElement("UserName")]
        public string UserName { get; set; }

        [BsonElement("Email")]
        public string? Email { get; set; }

        [BsonElement("MobileNumber")]
        public string? MobileNumber { get; set; }

        [BsonElement("Status")]
        public Status Status { get; set; }

        [BsonElement("Role")]
        public Role Role { get; set; }

        [BsonElement("PasswordHash")]
        public string PasswordHash { get; set; }


    }
}
