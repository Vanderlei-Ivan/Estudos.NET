using System.ComponentModel.DataAnnotations;

namespace ApiMongoTreino.Dtos.Products;

public class MakePaymentDto
{
    [Required(ErrorMessage = "Pedido é obrigatório")]
    public string RequestId { get; set; }
}
