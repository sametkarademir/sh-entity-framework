using SoftwareHospital.EntityFramework.Aggregates.Entities;

namespace SoftwareHospital.EntityFramework.Aggregates.AggregateRoots
{
    public interface IAggregateRoot : IEntity
    {
    
    }

    public interface IAggregateRoot<TKey> : IEntity<TKey>, IAggregateRoot
    {
    }
}