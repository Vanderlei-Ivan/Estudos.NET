using ApiMongoTreino.Dtos.Customes;
using ApiMongoTreino.Models;
using ApiMongoTreino.Service.Interface;
using MongoDB.Driver;

namespace ApiMongoTreino.Service;

public class CustomerService : ICustomerService
{
    private readonly IMongoCollection<Customer> _customers;
    public CustomerService(IMongoCollection<Customer> customers)
    {
        _customers = customers;
    }

    public async Task<ResponseCustomersDto> CreateCUstomer(CreateCustomerDto dto)
    {
        var customer = new Customer
        {
            Name = dto.Name,
            LastName = dto.LastName,
            Email = dto.Email,
            Phone = dto.Phone,
            Adress = dto.Adress
        };

        await _customers.InsertOneAsync(customer);
        return new ResponseCustomersDto
        {
            Name = customer.Name,
            LastName = customer.LastName,
            Adress = customer.Adress
        };
    }
}