using ApiMongoTreino.Dtos.Products;
using ApiMongoTreino.Models;
using ApiMongoTreino.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace ApiMongoTreino.Service;

public class ProductService : IProductService
{
    private readonly IMongoCollection<Product> _products;
    public ProductService(IMongoCollection<Product> products)
    {
        _products = products;
    }

    public async Task<CreateProductResponseDto> CreateProduct(CreateProductRequestDto dto)
    {
        var product = new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            Stock = dto.Stock
        };
        await _products.InsertOneAsync(product);

        return new CreateProductResponseDto
        {
            Menssage = "Produto Criado Com Sucesso",
            Name = product.Name,
            Description = product.Description
        };
    }

}