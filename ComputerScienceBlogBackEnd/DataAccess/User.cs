using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ComputerScienceBlogBackEnd.DataAccess
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Role { get; set; }

        public string Token { get; set; }
    }
}
