using ApiMongoTreino.Dtos.Products;

namespace ApiMongoTreino.Service.Payment.Strategies;

public interface IPaymentProcessor
{
    Task<ResponseBaseDto?> Process(MakePaymentDto dto);
}