namespace SoftwareHospital.EntityFramework.Core.Aggregates.Entities;

public interface IEntity
{
}

public interface IEntity<TKey> : IEntity
{
    TKey Id { get; }
}