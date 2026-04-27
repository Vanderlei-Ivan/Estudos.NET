using ApiMongoTreino.Dtos.Products;
using ApiMongoTreino.Enums.Requests;
using ApiMongoTreino.Service.Interface;

namespace ApiMongoTreino.Service.Payment.Strategies;

public class CreditCardPaymentProcessor : ApplicationService, IPaymentProcessor
{
    private readonly IRequestService _requestService;

    public CreditCardPaymentProcessor(IRequestService requestService, IApplicationNotificationHandler notifications) : base(notifications)
    {
        _requestService = requestService;
    }

    public async Task<ResponseBaseDto> Process(MakePaymentDto dto)
    {
        await _requestService.UpdateStatusRequest(RequestStatus.Paid, dto.RequestId);
        if (_notifications.HasErrors())
        {
            return ResponseBaseDto.Fail(_notifications.GetErrors());
        }
        return ResponseBaseDto.Ok("Pedido Pago Com Sucesso(card)");
    }
}