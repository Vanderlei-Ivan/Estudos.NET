using System.ComponentModel.DataAnnotations;
using ApiMongoTreino.Models;

namespace ApiMongoTreino.Dtos.Customes;

public class CreateCustomerDto
{
    [Required(ErrorMessage = "Campo Nome é Obrigatorio")]
    public string Name { get; set; }
    [Required(ErrorMessage = "Campo SobreNome é Obrigatorio")]
    public string LastName { get; set; }
    [Required(ErrorMessage = "Campo Email é Obrigatorio")]
    public string Email { get; set; }
    [Required(ErrorMessage = "Campo Telefone é Obrigatorio")]
    public string Phone { get; set; }
    [Required(ErrorMessage = "Por favor , nos informe seu Endereço")]
    public Address? Adress { get; set; }
}