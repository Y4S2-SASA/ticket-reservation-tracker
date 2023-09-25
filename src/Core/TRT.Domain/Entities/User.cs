using MongoDB.Bson.Serialization.Attributes;
using TRT.Domain.Enums;

namespace TRT.Domain.Entities
{
    public class User 
    {
        [BsonId]
        public string NIC { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public Role Role { get; set; }
        public string PasswordHash { get; set; }

    }
}
