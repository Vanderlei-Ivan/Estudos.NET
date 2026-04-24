using ApiMongoTreino.Enums.Requests;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ApiMongoTreino.Models;

public class Request
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public Customer Customer { get; set; }
    public List<ItensRequest> Itens { get; set;}
    public decimal TotalPrice { get; set;}
    public DateTime PaymentDate{ get; set;}
    public DateTime RequestDate{ get; set;}
    [BsonRepresentation(BsonType.String)]
    public RequestStatus Status { get; set; }
}