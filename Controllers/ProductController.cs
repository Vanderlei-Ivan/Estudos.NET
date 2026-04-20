using ApiMongoTreino.Dtos.Products;
using ApiMongoTreino.Service;
using ApiMongoTreino.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ApiMongoTreino.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _service;

    public ProductController(IProductService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequestDto dto)
    {
        var product = await _service.CreateProduct(dto);
        return Ok(product);

    }

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var products = await _service.GetProducts();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(string id)
    {
        try
        {
            var products = await _service.GetProductById(id);
            return Ok(products);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("search_for_filtered_products/{filter}")]
    public async Task<IActionResult> SearchForFilteredProducts(int filter)
    {
        var products = await _service.SearchForFilteredProducts(filter);
        
    }
}