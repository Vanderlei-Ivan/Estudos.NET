using ApiMongoTreino.Dtos.Products;
using ApiMongoTreino.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ApiMongoTreino.Controllers;

[ApiController]
[Route("[Controller]")]
public class RequestController
{
    private readonly IRequestService _service;
    private readonly IApplicationNotificationHandler _notifications;
    public RequestController(IRequestService service,IApplicationNotificationHandler notifications)
    {
        _service = service;
        _notifications = notifications;
    }

    [HttpPost]
    public async Task<IActionResult> CreateRequest([FromBody] CreateRequestDto dto)
    {
        var request = await _service.CreateRequest(dto);
    }
}