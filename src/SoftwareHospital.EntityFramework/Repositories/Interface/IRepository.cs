using SoftwareHospital.EntityFramework.Aggregates.Entities;

namespace SoftwareHospital.EntityFramework.Repositories.Interface;

public interface IRepository<TEntity, TKey> :
    IQuery<TEntity>,
    IReadRepository<TEntity, TKey>,
    IWriteRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
{
    
}

public interface IRepository<TEntity> :
    IQuery<TEntity>,
    IReadRepository<TEntity>,
    IWriteRepository<TEntity>
    where TEntity : class, IEntity
{
    
}