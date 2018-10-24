﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Netcore.RestAPI.Models
{
    public class Employee
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }

    }
}