using ApiMongoTreino.Dtos.Products;

namespace ApiMongoTreino.Service.Interface;

public interface IProductService
{
    Task<CreateProductResponseDto> CreateProduct(CreateProductRequestDto dto);
    Task<List<GetProductsDto>> GetProducts();
    Task<GetProductsDto?> GetProductById(string id);
    Task<List<GetProductsDto>> SearchForFilteredProducts(int filter);
    Task<List<GetProductsDto>> FilterProducts(FilterProductsDto dto);
}