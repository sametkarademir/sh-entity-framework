namespace SoftwareHospital.EntityFramework;

public interface IEventDispatcher
{
    Task DispatchAsync<TData>(TData domainEvent);
}