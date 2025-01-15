namespace SoftwareHospital.EntityFramework.Aggregates.BasicAggregateRoots;

public interface IEventDispatcher
{
    Task DispatchAsync<TData>(TData domainEvent);
}