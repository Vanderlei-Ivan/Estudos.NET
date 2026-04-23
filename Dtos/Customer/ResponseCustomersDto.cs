using System.ComponentModel.DataAnnotations;
using ApiMongoTreino.Models;

namespace ApiMongoTreino.Dtos.Customes;

public class ResponseCustomersDto
{
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public Address? Adress { get; set; }
}