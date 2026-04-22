using ApiMongoTreino.Dtos.Products;
using ApiMongoTreino.Models;
using ApiMongoTreino.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

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

// 1 - buscar produtos onde os preços sao maiores ou iguais a 100
// 2 - buscar produtos onde Existe estoque disponivel
    public async Task<List<GetProductsDto>> SearchForFilteredProducts(int filter)
    {
        List<Product> products = new();
        switch (filter)
        {
            case 1:
            products = await SearchForProductsByValue();
            break;

            case 2:
            products = await SearchForProductsExistsStock();
            break;

            default:
            throw new ArgumentException("Filtro inválido.");
        }
        return products.Select(p => new GetProductsDto
        {
            Name = p.Name,
            Price = p.Price,
            Description = p.Description,
            Stock = p.Stock
            
        }).ToList();
    }

    public async Task<List<Product>> SearchForProductsByValue()
    {
        return await _products.AsQueryable().Where(p => p.Price >= 100).ToListAsync();
    }

    public async Task<List<Product>> SearchForProductsExistsStock()
    {
        return await _products.AsQueryable().Where(p => p.Stock > 0).ToListAsync();
    }

    public async Task<List<GetProductsDto>> FilterProducts(FilterProductsDto filters)
    {
        var query = _products.AsQueryable();

        if (string.IsNullOrWhiteSpace(filters.Name))
        {
            query = query.Where(p => p.Name == filters.Name);
        }

        if (filters.Price.HasValue)
        {
            query = query.Where(p => p.Price <= filters.Price);
        }

        if (filters.Stock.HasValue)
        {
            query = query.Where(p => p.Stock >= filters.Stock);
        }
        
        var products = await query.ToListAsync();
        return products.Select(p => new GetProductsDto
        {
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            Stock = p.Stock
            
        }).ToList();

    }
}