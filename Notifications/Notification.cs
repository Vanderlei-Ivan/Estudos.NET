public class Notification
{
    private readonly List<string> _errors = new();

    public IReadOnlyCollection<string> Errors => _errors;

    public bool HasErrors => _errors.Any();

    public void AddError(string message)
    {
        _errors.Add(message);
    }

    public void Clear()
    {
        _errors.Clear();
    }
}