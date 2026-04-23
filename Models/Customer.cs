using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ApiMongoTreino.Models;

public class Customer
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string? Name { get; set;}
    public string? LastName { get; set;}
    public string? Email { get; set;}
    public string? Phone { get; set;}
    public Address? Address { get; set;}
}