using ApiMongoTreino.Dtos.Customes;
using ApiMongoTreino.Dtos.Products;
using ApiMongoTreino.Enums.Requests;
using ApiMongoTreino.Models;
using ApiMongoTreino.Service.Interface;
using MongoDB.Driver;

namespace ApiMongoTreino.Service;

public class RequestService : ApplicationService, IRequestService
{
    public readonly IMongoCollection<Request> _requests;
    public readonly ICustomerService _customerService;
    public readonly IProductService _productService;
    public RequestService(IMongoCollection<Request> requests,
    ICustomerService customerService,
    IProductService productService,
    IApplicationNotificationHandler notifications) : base(notifications)
    {
        _requests = requests;
        _customerService = customerService;
        _productService = productService;
    }

    public async Task<ResponseBaseDto?> CreateRequest(CreateRequestDto dto)
    {
        var customer = await _customerService.GetCustomer(dto.CustomerId);
        if (customer is null)
        {
            return null;
        }

        var products = await _productService.GetProductsForRequest(dto.Itens);

        if (_notifications.HasErrors())
        {
            return null;
        }

        await RegisterOrder(products, customer, dto);
        await _productService.UpdateStock(products,dto);

        return new ResponseBaseDto
        {
            Success = true,
            Message = "Pedido Realizado com Sucesso!"
        };
    }

    public async Task RegisterOrder(List<Product> products, Customer customer, CreateRequestDto dto)
    {
        var productsById = products.ToDictionary(p => p.Id);

        var itensRequest = dto.Itens.Select(item =>
        {
            var product = productsById[item.ProductId];

            return new ItensRequest
            {
                Id = product.Id,
                ProductName = product.Name,
                Quantity = item.Quantity,
                UnitPrice = product.Price
            };
        }).ToList();

        var request = new Request
        {
            Customer = customer,
            Itens = itensRequest,
            TotalPrice = (decimal)itensRequest.Sum(i => i.SubTotalPrice),
            RequestDate = DateTime.UtcNow,
            Status = RequestStatus.Pending
        };

        await _requests.InsertOneAsync(request);
    }
}