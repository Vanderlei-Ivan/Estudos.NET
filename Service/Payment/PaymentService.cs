using ApiMongoTreino.Dtos.Products;
using ApiMongoTreino.Enums.Requests;
using ApiMongoTreino.Service.Interface;
using ApiMongoTreino.Service.Payment.Factories;

namespace ApiMongoTreino.Service.Payment;

public class PaymentService : ApplicationService,IPaymentService
{
    private readonly IRequestService _requestService;
    private readonly IPaymentProcessorFactory _factory;
    public PaymentService(IPaymentProcessorFactory factory, IRequestService requestService, IApplicationNotificationHandler notifications)
        : base(notifications)
    {
        _requestService = requestService;
        _factory = factory;
    }

    public async Task<ResponseBaseDto> MakePayment(MakePaymentDto dto)
    {
        var request = await _requestService.GetPaymentMethod(dto.RequestId);
        if (request == null)
        {
            _notifications.HandleError(new ApplicationNotification("Pedido não Encontrado"));
            return ResponseBaseDto.Fail(_notifications.GetErrors());
        }

        if (request.Status != RequestStatus.PendingPayment)
        {
            _notifications.HandleError(new ApplicationNotification("o Pagamento não pode ser realizado, pois o status do pedido não esta Pendente"));
            return ResponseBaseDto.Fail(_notifications.GetErrors());
        }
        var processor =
         _factory.GetProcessor(
             request.PaymentMethod
         );

        return await processor.Process(dto);
    }
}