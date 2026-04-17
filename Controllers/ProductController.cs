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
}