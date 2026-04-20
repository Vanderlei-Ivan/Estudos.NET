using System.ComponentModel.DataAnnotations;

namespace ApiMongoTreino.Dtos.Products;

public class GetProductsDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }

}