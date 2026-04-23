using ApiMongoTreino.Dtos.Customes;

namespace ApiMongoTreino.Service.Interface; 

public interface ICustomerService
{
    Task<ResponseCustomersDto?> CreateCustomer(CreateCustomerDto dto);
    Task<List<ResponseCustomersDto>> GetCustomers();
    Task<ResponseCustomersDto?> GetCustomerById(string id);
}