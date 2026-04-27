using ApiMongoTreino.Dtos.Products;
using ApiMongoTreino.Enums.Requests;
using ApiMongoTreino.Models;

namespace ApiMongoTreino.Service.Interface;

public interface IRequestService
{
    Task<ResponseBaseDto?> CreateRequest(CreateRequestDto dto);
    Task<Request> GetPaymentMethod(string Id);
    Task UpdateStatusRequest(RequestStatus StatusNew, string RequestId);

}