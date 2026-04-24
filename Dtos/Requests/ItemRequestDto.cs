using System.ComponentModel.DataAnnotations;

namespace ApiMongoTreino.Dtos.Products;

public class ItemRequestDto
{
    [Required(ErrorMessage = "Produto é obrigatorio")]
    public string ProductId { get; set; }
    [Required(ErrorMessage = "A quantidade é obrigatória")]
    public int Quantity { get; set; }
}
