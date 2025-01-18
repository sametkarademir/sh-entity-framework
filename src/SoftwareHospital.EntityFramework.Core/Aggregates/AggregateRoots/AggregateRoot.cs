using SoftwareHospital.EntityFramework.Core.Aggregates.BasicAggregateRoots;

namespace SoftwareHospital.EntityFramework.Core.Aggregates.AggregateRoots;

public abstract class AggregateRoot : BasicAggregateRoot, IHasExtraProperties, IHasConcurrencyStamp
{
    public virtual ExtraPropertyDictionary ExtraProperties { get; protected set; }
    public virtual string ConcurrencyStamp { get; set; }
    
    protected AggregateRoot()
    {
        ConcurrencyStamp = Guid.NewGuid().ToString("N");
        ExtraProperties = new ExtraPropertyDictionary();
    }
}

[Serializable]
public abstract class AggregateRoot<TKey> : BasicAggregateRoot<TKey>, IHasExtraProperties, IHasConcurrencyStamp
{
    public virtual ExtraPropertyDictionary ExtraProperties { get; protected set; }
    public virtual string ConcurrencyStamp { get; set; }

    protected AggregateRoot()
    {
        ConcurrencyStamp = Guid.NewGuid().ToString("N");
        ExtraProperties = new ExtraPropertyDictionary();
    }

    protected AggregateRoot(TKey id)
    {
        ConcurrencyStamp = Guid.NewGuid().ToString("N");
        ExtraProperties = new ExtraPropertyDictionary();
    }
}