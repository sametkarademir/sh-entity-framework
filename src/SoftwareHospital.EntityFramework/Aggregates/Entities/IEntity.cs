namespace SoftwareHospital.EntityFramework.Aggregates.Entities;

public interface IEntity
{
}

public interface IEntity<TKey> : IEntity
{
    TKey Id { get; }
}