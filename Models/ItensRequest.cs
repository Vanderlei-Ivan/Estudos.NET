using ApiMongoTreino.Enums.Requests;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ApiMongoTreino.Models;

public class ItensRequest
{
    public string? Id { get; set; }
    public string? ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal SubTotalPrice => UnitPrice * Quantity;
}
