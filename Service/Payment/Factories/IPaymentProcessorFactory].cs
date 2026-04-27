using ApiMongoTreino.Enums.Requests;
using ApiMongoTreino.Service.Payment.Strategies;

namespace ApiMongoTreino.Service.Payment.Factories;

public interface IPaymentProcessorFactory
{
    IPaymentProcessor GetProcessor(PaymentMethod method);
}