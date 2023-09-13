using MongoDB.Bson.Serialization.Attributes;

namespace TRT.Domain.Entities
{
    public class User 
    {
        [BsonId]
        public string NIC { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

    }
}
