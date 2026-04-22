using System.ComponentModel.DataAnnotations;

namespace ApiMongoTreino.Dtos.Products;

public class FilterProductsDto
{
    public string? Name { get; set; }
    public decimal? Price { get; set; }
    public int? Stock { get; set; }

}