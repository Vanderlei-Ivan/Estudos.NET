using System.ComponentModel.DataAnnotations;

namespace ApiMongoTreino.Dtos.Products;

public class CreateProductRequestDto
{
    [Required(ErrorMessage = "O nome é obrigatório")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "A descrição é obrigatória")]
    public string? Description { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "O preço deve ser maior que zero")]
    public decimal Price { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "O estoque deve ser maior que zero")]
    public int Stock { get; set; }

}