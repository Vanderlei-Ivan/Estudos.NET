namespace ApiMongoTreino.Dtos.Products;

public class ResponseBaseDto
{
    public bool Success { get; set; }

    public string? Message { get; set; }

    public List<string> Errors { get; set; } = new();

    public static ResponseBaseDto Ok(string message)
    {
        return new ResponseBaseDto
        {
            Success = true,
            Message = message
        };
    }

    public static ResponseBaseDto Fail(
    List<ApplicationNotification> notifications)
    {
        return new ResponseBaseDto
        {
            Success = false,
            Message = "Operação não realizada",
            Errors = notifications
                .Select(x => x.Message)
                .ToList()
        };
    }
}
