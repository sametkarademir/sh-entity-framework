namespace SoftwareHospital.EntityFramework.Core.Aggregates.BasicAggregateRoots;

public interface IGeneratesDomainEvents
{
    IEnumerable<DomainEventRecord> GetLocalEvents();

    IEnumerable<DomainEventRecord> GetDistributedEvents();

    void ClearLocalEvents();

    void ClearDistributedEvents();

    void AddLocalEvent(object eventData);
    void AddDistributedEvent(object eventData);
}