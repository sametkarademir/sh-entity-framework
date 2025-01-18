using SoftwareHospital.EntityFramework.Core.Aggregates.Entities;

namespace SoftwareHospital.EntityFramework.Core.Aggregates.AggregateRoots
{
    public interface IAggregateRoot : IEntity
    {
    
    }

    public interface IAggregateRoot<TKey> : IEntity<TKey>, IAggregateRoot
    {
    }
}