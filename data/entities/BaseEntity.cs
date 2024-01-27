using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace data.entities;

public interface IEntity
{
    string Id { get; }
    DateTime CreatedDate { get; set; }
    bool IsActive { get; set; }
}

public class BaseEntity : IEntity
{
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement(Order = 0)]
    [BsonId]
    public string Id { get; } = ObjectId.GenerateNewId().ToString();

    [BsonRepresentation(BsonType.DateTime)]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    [BsonElement(Order = 1)]
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;


    [BsonElement(Order = 3)]
    public bool IsActive { get; set; }

    /* Kimlik doğrulama ile kullanılan property
    [BsonElement(Order = 4)]
    public int CreatedBy { get; set; } */
}