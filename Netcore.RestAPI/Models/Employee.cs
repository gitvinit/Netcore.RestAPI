using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Netcore.RestAPI.Models
{
    public class Employee
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Name { get; set; }

    }
}