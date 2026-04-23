using ApiMongoTreino.Dtos.Customes;
using ApiMongoTreino.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ApiMongoTreino.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _service;
    private readonly IApplicationNotificationHandler _notifications;

    public CustomerController(
        ICustomerService service,
        IApplicationNotificationHandler notifications)
    {
        _service = service;
        _notifications = notifications;
    }

    [HttpPost]
    public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerDto dto)
    {
        var customer = await _service.CreateCustomer(dto);

        if (_notifications.HasErrors())
            return BadRequest(_notifications.GetErrors());

        return Ok(customer);
    }

    [HttpGet]
    public async Task<IActionResult> GetCustomers()
    {
        var customers = await _service.GetCustomers();
        return Ok(customers);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCustomerById(string id)
    {
        var customer = await _service.GetCustomerById(id);

        if (_notifications.HasErrors())
            return BadRequest(_notifications.GetErrors());

        return Ok(customer);
    }
}