using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Auth.Domain.Services.User.Entities
{
    public class UserEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public string Email { get; set; }

        public string Name { get; set; }
        public string PasswordHash { get; set; }
        public string Gender { get; set; }
        public DateTime Birth { get; set; }
    }
}