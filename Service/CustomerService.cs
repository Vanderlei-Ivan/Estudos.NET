using ApiMongoTreino.Dtos.Customes;
using ApiMongoTreino.Models;
using ApiMongoTreino.Service.Interface;
using MongoDB.Driver;

namespace ApiMongoTreino.Service;

public class CustomerService : ApplicationService, ICustomerService
{
    private readonly IMongoCollection<Customer> _customers;

    public CustomerService(
        IMongoCollection<Customer> customers,
        IApplicationNotificationHandler notifications)
        : base(notifications)
    {
        _customers = customers;
    }

    public async Task<List<ResponseCustomersDto>> GetCustomers()
    {
        var customers = await _customers.Find(_ => true).ToListAsync();
        return customers.Select(c => new ResponseCustomersDto
        {
            Name = c.Name,
            LastName = c.LastName,
            Email = c.Email,
            Phone = c.Phone,
            Address = new AddressDto
            {
                Street = c.Address.Street,
                City = c.Address.City,
                ZipCode = c.Address.ZipCode,
                Number = c.Address.Number,
                State = c.Address.State
            }

        }).ToList();
    }

    public async Task<ResponseCustomersDto?> CreateCustomer(CreateCustomerDto dto)
    {
        await ApplyValidationsCreateNewUser(dto);
        if (_notifications.HasErrors())
        {
            return null;
        }
        var customer = new Customer
        {
            Name = dto.Name,
            LastName = dto.LastName,
            Email = dto.Email,
            Phone = dto.Phone,
            Address = new Address
            {
                Street = dto.Address.Street,
                City = dto.Address.City,
                ZipCode = dto.Address.ZipCode,
                Number = dto.Address.Number,
                State = dto.Address.State
            }
        };

        await _customers.InsertOneAsync(customer);
        return new ResponseCustomersDto
        {
            Name = customer.Name,
            LastName = customer.LastName,
            Address = new AddressDto
            {
                Street = customer.Address.Street,
                City = customer.Address.City,
                ZipCode = customer.Address.ZipCode,
                Number = customer.Address.Number,
                State = customer.Address.State
            }
        };
    }

    public async Task<ResponseCustomersDto?> GetCustomerById(string id)
    {
        var customer = await GetCustomer(id);
        if(customer is null)
        {
            return null;
        }
        return new ResponseCustomersDto
        {
            Name = customer.Name,
            LastName = customer.LastName,
            Email = customer.Email,
            Phone = customer.Email,
            Address = new AddressDto
            {
                Street = customer.Address.Street,
                Number = customer.Address.Number,
                City = customer.Address.City,
                State = customer.Address.State,
                ZipCode = customer.Address.ZipCode
            }

        };
    }

    public async Task<Customer?> GetCustomer(string id)
    {
        var customer = await _customers.Find(c => c.Id == id).FirstOrDefaultAsync();
        if (customer == null)
        {
            _notifications.HandleError(new ApplicationNotification("Customer não identificado"));
            return null;
        }
        return customer;
        
    }

    public async Task ApplyValidationsCreateNewUser(CreateCustomerDto dto)
    {
        var emailExists = await _customers
            .Find(c => c.Email == dto.Email)
            .AnyAsync();

        var phoneExists = await _customers
            .Find(c => c.Phone.Trim() == dto.Phone.Trim())
            .AnyAsync();

        if (emailExists)
        {
            _notifications.HandleError(new ApplicationNotification("Email já existe na base de dados"));
            return;
        }

        if (phoneExists)
        {
            _notifications.HandleError(new ApplicationNotification("telefone já existe na base de dados"));
            return;
        }

    }
}