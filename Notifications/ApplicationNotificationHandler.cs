public class ApplicationNotificationHandler : IApplicationNotificationHandler
{
    private readonly List<ApplicationNotification> _notifications = new();

    public void HandleError(ApplicationNotification message)
    {
        message.IsError = true;
        _notifications.Add(message);
    }

    public bool HasErrors() => _notifications.Any(x => x.IsError);

    public List<ApplicationNotification> GetErrors()
        => _notifications.Where(x => x.IsError).ToList();

    public List<ApplicationNotification> GetNotifications()
        => _notifications;

    public bool HasSuccess()
        => _notifications.Any(x => !x.IsError);

    public List<ApplicationNotification> GetSuccesses()
        => _notifications.Where(x => !x.IsError).ToList();

    public void HandleSuccess(ApplicationNotification message)
    {
        message.IsError = false;
        _notifications.Add(message);
    }

    public void Clean() => _notifications.Clear();

    public void CleanErrors()
        => _notifications.RemoveAll(x => x.IsError);

    public void CleanSuccessess()
        => _notifications.RemoveAll(x => !x.IsError);

    public void ThrowsNotificaton()
    {
        if (HasErrors())
            throw new Exception("Existem notificações de erro");
    }
}