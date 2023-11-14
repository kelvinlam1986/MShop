﻿using MongoDB.Bson.Serialization.Attributes;

namespace MShop.Infrastructure.Event.User
{
    public class UserCreated
    {
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        [BsonId]
        public string UserId { get; set; }
        public string Username { get; set; }
        public string EmailId { get; set; }
        public string ContactNo { get; set; }
        public string Password { get; set; }
    }
}
