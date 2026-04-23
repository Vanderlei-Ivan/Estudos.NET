using ApiMongoTreino.Dtos.Products;

namespace ApiMongoTreino.Service.Interface;

public interface IRequestService
{
    Task<ResponseRequestDto?> CreateRequest(CreateRequestDto dto);
}