using ApiMongoTreino.Enums.Requests;
using ApiMongoTreino.Service.Payment.Strategies;

namespace ApiMongoTreino.Service.Payment.Factories;

public class PaymentProcessorFactory
    : IPaymentProcessorFactory
{
    private readonly IServiceProvider _provider;

    public PaymentProcessorFactory(
       IServiceProvider provider)
    {
        _provider = provider;
    }

    public IPaymentProcessor GetProcessor(
        PaymentMethod method)
    {
        return method switch
        {
            PaymentMethod.Pix =>
                _provider.GetRequiredService
                   <PixPaymentProcessor>(),

            PaymentMethod.CreditCard =>
                _provider.GetRequiredService
                   <CreditCardPaymentProcessor>(),

            PaymentMethod.Ticket =>
                _provider.GetRequiredService
                   <TicketPaymentProcessor>(),

            _ => throw new Exception()
        };
    }
}