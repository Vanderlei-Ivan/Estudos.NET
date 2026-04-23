using ApiMongoTreino.Dtos.Products;
using ApiMongoTreino.Models;
using ApiMongoTreino.Service.Interface;
using MongoDB.Driver;

namespace ApiMongoTreino.Service;

public class RequestService : ApplicationService,IRequestService
{
    public readonly IMongoCollection<Request> _requests;
    public readonly CustomerService _customerService;
    public readonly ProductService _productService;
    public RequestService(IMongoCollection<Request> requests,
    CustomerService customerService,
    ProductService productService,
    IApplicationNotificationHandler notifications) : base(notifications)
    {
        _requests = requests;
        _customerService = customerService;
        _productService = productService;
    }

    public async Task<ResponseRequestDto?> CreateRequest(CreateRequestDto dto)
    {
        var customer = await _customerService.GetCustomer(dto.CustomerId);
        if (customer is null)
        {
            return null;
        }
        // implementar logica dos produtos (se um produto nao existir retornar erro ou remover da lista de pedidos)

        var products = await _productService.
    }
    
}