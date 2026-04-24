using ApiMongoTreino.Dtos.Products;

namespace ApiMongoTreino.Service.Interface;

public interface IRequestService
{
    Task<ResponseBaseDto?> CreateRequest(CreateRequestDto dto);
}