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

    public async Task<List<GetProductsDto>> GetProducts()
    {
       var products =  await _products.Find(_ => true).ToListAsync();
       return products.Select(p => new GetProductsDto
       {
           Name = p.Name,
           Price = p.Price,
           Description = p.Description,
           Stock = p.Stock

       }).DistinctBy(p => new{p.Name,p.Price,p.Stock}).ToList();
    }

    public async Task<GetProductsDto> GetProductById(string id)
    {
        var product = await _products.Find(p => p.Id == id).FirstOrDefaultAsync();
        if (product == null)
        {
            throw new Exception("Produto não encontrado");
        }
        return new GetProductsDto
        {
            Name = product.Name,
            Price = product.Price
        };
    }

    public async Task<List<GetProductsDto>> SearchForFilteredProducts(int filter)
    {
        var products = SearchProductsByFilter(filter);
    }

    public async Task<List<GetProductsDto>> SearchProductsByFilter(int filter)
    {
        
    }
}