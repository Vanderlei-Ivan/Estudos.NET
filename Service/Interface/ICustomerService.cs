using ApiMongoTreino.Dtos.Customes;
using ApiMongoTreino.Models;

namespace ApiMongoTreino.Service.Interface; 

public interface ICustomerService
{
    Task<ResponseCustomersDto?> CreateCustomer(CreateCustomerDto dto);
    Task<List<ResponseCustomersDto>> GetCustomers();
    Task<ResponseCustomersDto?> GetCustomerById(string id);
    Task<Customer?> GetCustomer(string id);
}