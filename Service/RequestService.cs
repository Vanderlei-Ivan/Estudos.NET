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
        if (!Enum.IsDefined(typeof(PaymentMethod), dto.PaymentMethod))
        {
            _notifications.HandleError(new ApplicationNotification("Metodo de pagamento Invalido"));
            return null;
        }
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
        await _productService.UpdateStock(dto);

        return ResponseBaseDto.Ok("Pedido Realizado com Sucesso!");
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
            Status = RequestStatus.PendingPayment,
            PaymentMethod = dto.PaymentMethod
        };

        await _requests.InsertOneAsync(request);
    }

    public async Task<Request> GetPaymentMethod(string Id)
    {
        return await _requests.Find(r => r.Id == Id).FirstOrDefaultAsync();
    }

    public async Task UpdateStatusRequest(
    RequestStatus statusNew,
    string requestId)
    {
        try
        {
            var filter = Builders<Request>.Filter
                .Eq(r => r.Id, requestId);

            var update = Builders<Request>.Update
                .Set(r => r.Status, statusNew)
                .Set(r => r.PaymentDate, DateTime.UtcNow);

            var result = await _requests.UpdateOneAsync(
                filter,
                update
            );

            if (result.MatchedCount == 0)
            {
                _notifications.HandleError(
                    new ApplicationNotification(
                        "pedido não encontrado."
                    )
                );
            }
        }
        catch (MongoException ex)
        {
            _notifications.HandleError(
                new ApplicationNotification(
                    $"Erro ao atualizar pagamento: {ex.Message}"
                )
            );
        }
    }
}