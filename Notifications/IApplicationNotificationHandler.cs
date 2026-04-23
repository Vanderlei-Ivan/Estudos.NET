public interface IApplicationNotificationHandler
{
    bool HasErrors();
    bool HasSuccess();
    List<ApplicationNotification> GetNotifications();
    List<ApplicationNotification> GetErrors();
    List<ApplicationNotification> GetSuccesses();
    void Clean();
    void CleanSuccessess();
    void CleanErrors();
    void HandleError(ApplicationNotification message);
    void HandleSuccess(ApplicationNotification message);
    void ThrowsNotificaton();
}