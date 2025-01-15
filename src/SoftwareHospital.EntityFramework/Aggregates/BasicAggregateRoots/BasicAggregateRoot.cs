using System.Collections.ObjectModel;
using SoftwareHospital.EntityFramework.Aggregates.AggregateRoots;
using SoftwareHospital.EntityFramework.Aggregates.Entities;
using SoftwareHospital.EntityFramework.Extensions;

namespace SoftwareHospital.EntityFramework.Aggregates.BasicAggregateRoots;

[Serializable]
public abstract class BasicAggregateRoot : Entity, IAggregateRoot, IGeneratesDomainEvents
{
    private readonly ICollection<DomainEventRecord> _distributedEvents = new Collection<DomainEventRecord>();
    private readonly ICollection<DomainEventRecord> _localEvents = new Collection<DomainEventRecord>();

    public virtual IEnumerable<DomainEventRecord> GetLocalEvents()
    {
        return _localEvents;
    }

    public virtual IEnumerable<DomainEventRecord> GetDistributedEvents()
    {
        return _distributedEvents;
    }

    public virtual void ClearLocalEvents()
    {
        _localEvents.Clear();
    }

    public virtual void ClearDistributedEvents()
    {
        _distributedEvents.Clear();
    }

    public virtual void AddLocalEvent(object eventData)
    {
        _localEvents.Add(new DomainEventRecord(eventData, EventOrderGeneratorExtensions.GetNext()));
    }

    public virtual void AddDistributedEvent(object eventData)
    {
        _distributedEvents.Add(new DomainEventRecord(eventData, EventOrderGeneratorExtensions.GetNext()));
    }
}

[Serializable]
public abstract class BasicAggregateRoot<TKey> : Entity<TKey>,
    IAggregateRoot<TKey>,
    IGeneratesDomainEvents
{
    private readonly ICollection<DomainEventRecord> _distributedEvents = new Collection<DomainEventRecord>();
    private readonly ICollection<DomainEventRecord> _localEvents = new Collection<DomainEventRecord>();

    protected BasicAggregateRoot()
    {

    }

    protected BasicAggregateRoot(TKey id)
        : base(id)
    {

    }

    public virtual IEnumerable<DomainEventRecord> GetLocalEvents()
    {
        return _localEvents;
    }

    public virtual IEnumerable<DomainEventRecord> GetDistributedEvents()
    {
        return _distributedEvents;
    }

    public virtual void ClearLocalEvents()
    {
        _localEvents.Clear();
    }

    public virtual void ClearDistributedEvents()
    {
        _distributedEvents.Clear();
    }

    public virtual void AddLocalEvent(object eventData)
    {
        _localEvents.Add(new DomainEventRecord(eventData, EventOrderGeneratorExtensions.GetNext()));
    }

    public virtual void AddDistributedEvent(object eventData)
    {
        _distributedEvents.Add(new DomainEventRecord(eventData, EventOrderGeneratorExtensions.GetNext()));
    }
}