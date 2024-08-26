using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BookStoreApi.Models;

public interface IBaseEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    string? Id { get; set; }

}