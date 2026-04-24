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
    private readonly IApplicationNotificationHandler _notifications; 
    public ProductController(IProductService service,IApplicationNotificationHandler notifications)
    {
        _service = service;
        _notifications = notifications;
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequestDto dto)
    {
        var product = await _service.CreateProduct(dto);

        if (_notifications.HasErrors())
        {
            return BadRequest(_notifications.GetErrors());
        }
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
        var products = await _service.GetProductById(id);
        
        if (_notifications.HasErrors())
        {
            return BadRequest(_notifications.GetErrors());
        }

        return Ok(products);
    }

    [HttpGet("search_for_filtered_products/{filter}")]
    public async Task<IActionResult> SearchForFilteredProducts(int filter)
    {
        try
        {
            var products = await _service.SearchForFilteredProducts(filter);
            return Ok(products);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }

    }

    [HttpGet("filter_products")]
    public async Task<IActionResult> FilterProducts([FromQuery] FilterProductsDto dto)
    {
        var products = await _service.FilterProducts(dto);
        return Ok(products);
    }
}