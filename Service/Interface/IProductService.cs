using ApiMongoTreino.Dtos.Products;

namespace ApiMongoTreino.Service.Interface;

public interface IProductService
{
    Task<CreateProductResponseDto> CreateProduct(CreateProductRequestDto dto);
}