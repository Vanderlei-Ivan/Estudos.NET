using System.ComponentModel.DataAnnotations;
using ApiMongoTreino.Dtos.Customes;

namespace ApiMongoTreino.Dtos.Products;

public class CreateRequestDto
{
    [Required(ErrorMessage = "Cliente é obrigatorio")]
    public string CustomerId { get; set; }

    [Required(ErrorMessage = "A descrição é obrigatória")]
    public List<string> ProductId { get; set; }
}
