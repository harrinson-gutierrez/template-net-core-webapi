namespace Application.Interfaces.Resources
{
    public interface IAppResource
    {
        string this[string key] { get; }
        string this[string key, object[] arguments] { get; }
    }
}
