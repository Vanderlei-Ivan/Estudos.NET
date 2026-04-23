public class ApplicationService
{
    protected readonly IApplicationNotificationHandler _notifications;

    public ApplicationService(IApplicationNotificationHandler notifications)
    {
        _notifications = notifications;
    }
}