using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BookStoreApi.Models;

public class Magazine : BaseEntity
{
    [BsonElement("Name")]
    public string MagazineName { get; set; } = null!;

    public decimal ?Price { get; set; }

    public string Author { get; set; } = null!;
}