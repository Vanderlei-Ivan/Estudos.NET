using System.ComponentModel.DataAnnotations;
using ApiMongoTreino.Dtos.Customes;
using ApiMongoTreino.Enums.Requests;

namespace ApiMongoTreino.Dtos.Products;

public class ResponseRequestDto
{
    public ResponseCustomersDto Customer { get; set; }
    public List<GetProductsDto> Products { get; set; }
    public int TotalPrice { get; set;}
    public DateTime RequestDate{ get; set;}
    public RequestStatus Status { get; set; }
}
