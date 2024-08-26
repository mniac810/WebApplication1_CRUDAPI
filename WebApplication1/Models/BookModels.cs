using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BookStoreApi.Models;

public class Book : BaseEntity
{
    [BsonElement("Name")]
    public string BookName { get; set; } = null!;

    public decimal? Price { get; set; }

    public string Category { get; set; } = null!;

    public string Author { get; set; } = null!;

    public Dictionary<string, string> CustomFields { get; set; } = null!;
}