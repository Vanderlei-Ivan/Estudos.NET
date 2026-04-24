using System.ComponentModel;
using System.ComponentModel.Design;
using ApiMongoTreino.Dtos.Products;
using ApiMongoTreino.Models;
using ApiMongoTreino.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace ApiMongoTreino.Service;

public class ProductService : ApplicationService, IProductService
{
    private readonly IMongoCollection<Product> _products;
    public ProductService(IMongoCollection<Product> products, IApplicationNotificationHandler notifications)
        : base(notifications)
    {
        _products = products;
    }

    public async Task<CreateProductResponseDto?> CreateProduct(CreateProductRequestDto dto)
    {
        var product = await AssembleProductObject(dto);
        if (product is null)
        {
            return null;
        }

        await _products.InsertOneAsync(product);

        return new CreateProductResponseDto
        {
            Menssage = "Produto Criado Com Sucesso",
            Name = product.Name,
            Description = product.Description
        };
    }

    public async Task<Product?> AssembleProductObject(CreateProductRequestDto dto)
    {
        var productExists = await _products.Find(p => p.Name == dto.Name
        && p.Description == dto.Description
        && p.Price == dto.Price).AnyAsync();

        if (productExists)
        {
            _notifications.HandleError(new ApplicationNotification("Produto já Existente"));
            return null;
        }

        return new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            Stock = dto.Stock
        };

    }

    public async Task<List<GetProductsDto>> GetProducts()
    {
        var products = await _products.Find(_ => true).ToListAsync();
        return products.Select(p => new GetProductsDto
        {
            Name = p.Name,
            Price = p.Price,
            Description = p.Description,
            Stock = p.Stock

        }).DistinctBy(p => new { p.Name, p.Price, p.Stock }).ToList();
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

    public async Task<GetProductsDto?> GetProductById(string id)
    {
        var product = await GetProduct(id);
        if (product is null)
        {
            return null;
        }
        return new GetProductsDto
        {
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock
        };
    }

    public async Task<Product?> GetProduct(string id)
    {
        var product = await _products.Find(p => p.Id == id).FirstOrDefaultAsync();
        if (product is null)
        {
            _notifications.HandleError(new ApplicationNotification("Produto não encontrado"));
            return null;
        }
        return product;
    }

    public async Task<List<Product>> GetProductsForRequest(List<ItemRequestDto> items)
    {
        var productIds = items
            .Select(i => i.ProductId)
            .Distinct()
            .ToList();

        var products = await _products
            .Find(p => productIds.Contains(p.Id))
            .ToListAsync();

        foreach (var item in items)
        {
            var product = products.FirstOrDefault(p => p.Id == item.ProductId);

            if (product is null)
            {
                _notifications.HandleError(
                    new ApplicationNotification($"Produto {item.ProductId} não encontrado.")
                );
                return new List<Product>();
            }

            if (product.Stock < item.Quantity)
            {
                _notifications.HandleError(
                    new ApplicationNotification(
                       $"Produto {product.Name} sem estoque suficiente."
                    )
                );
                return new List<Product>();
            }
        }

        return products;
    }

    public async Task UpdateStock(List<Product> products, CreateRequestDto dto)
    {
        foreach (var item in dto.Itens)
        {
            var filter = Builders<Product>.Filter
                .Eq(p => p.Id, item.ProductId);

            var update = Builders<Product>.Update
                .Inc(p => p.Stock, -item.Quantity);

            await _products.UpdateOneAsync(filter, update);
        }
    }
    // public async Task UpdateStock(List<Product> products, CreateRequestDto dto)
    // {
    //     foreach (var item in dto.Itens)
    //     {
    //         var filter = await _products.Find(p => p.Id == item.ProductId).FirstOrDefaultAsync();
    //         var NewStock = filter.Stock - item.Quantity;

    //         filter.Stock = NewStock;

    //         await _products.ReplaceOneAsync(p => p.Id == item.ProductId, filter);
    //     }
    // }
}