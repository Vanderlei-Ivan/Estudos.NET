using ApiMongoTreino.Dtos.Products;

namespace ApiMongoTreino.Service.Payment;

public interface IPaymentService
{
   Task<ResponseBaseDto> MakePayment(MakePaymentDto dto);
}