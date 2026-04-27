using System.ComponentModel.DataAnnotations;
using ApiMongoTreino.Dtos.Customes;
using ApiMongoTreino.Enums.Requests;

namespace ApiMongoTreino.Dtos.Products;

public class CreateRequestDto
{
    [Required(ErrorMessage = "Cliente é obrigatorio")]
    public string CustomerId { get; set; }

    [Required(ErrorMessage = "A descrição é obrigatória")]
    public List<ItemRequestDto> Itens { get; set; }

    [Required(ErrorMessage = "Por favor, informe o metodo de pagamento")]
    public PaymentMethod PaymentMethod { get; set; } 
}
