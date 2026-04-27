using ApiMongoTreino.Dtos.Products;
using ApiMongoTreino.Service.Payment;
using Microsoft.AspNetCore.Mvc;

namespace ApiMongoTreino.Controllers;

[ApiController]
[Route("[Controller]")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _service;
    private readonly IApplicationNotificationHandler _notifications;


    public PaymentController(IPaymentService service, IApplicationNotificationHandler notifications)
    {
        _service = service;
        _notifications = notifications;
    }


    [HttpPost("pay")]
    public async Task<IActionResult> Pay([FromBody] MakePaymentDto dto)
    {
        var result = await _service.MakePayment(dto);
        if (_notifications.HasErrors())
        {
            return BadRequest(result);
        }
        return Ok(result);

    }
}