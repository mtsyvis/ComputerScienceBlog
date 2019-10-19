using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ComputerScienceBlogBackEnd.DataAccess
{
    public class Article
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public string CreatedUser { get; set; }

        public DateTime CreatedDate { get; set; }

        public Category[] Categories { get; set; }

        [BsonIgnoreIfNull]
        public Comment[] Comments { get; set; }
    }
}
