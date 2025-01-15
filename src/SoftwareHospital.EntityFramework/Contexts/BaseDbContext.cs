using System.Linq.Expressions;
using System.Reflection;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SoftwareHospital.EntityFramework.Aggregates.AggregateRoots;
using SoftwareHospital.EntityFramework.Aggregates.AuditedAggregateRoots;
using SoftwareHospital.EntityFramework.Aggregates.BasicAggregateRoots;
using SoftwareHospital.EntityFramework.Aggregates.CreationAuditedAggregateRoots;
using SoftwareHospital.EntityFramework.Aggregates.Entities;
using SoftwareHospital.EntityFramework.Aggregates.FullAuditedAggregateRoots;

namespace SoftwareHospital.EntityFramework.Contexts;

public abstract class BaseDbContext : DbContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IEventDispatcher _eventDispatcher;

    protected BaseDbContext(DbContextOptions options, IHttpContextAccessor httpContextAccessor, IEventDispatcher eventDispatcher) : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
        _eventDispatcher = eventDispatcher;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
            {
                var parameter = Expression.Parameter(entityType.ClrType, "entity");
                var filter = Expression.Lambda(
                    Expression.Equal(
                        Expression.Property(parameter, nameof(ISoftDelete.IsDeleted)),
                        Expression.Constant(false)
                    ),
                    parameter
                );

                builder.Entity(entityType.ClrType).HasQueryFilter(filter);
            }
            
            if (typeof(IHasConcurrencyStamp).IsAssignableFrom(entityType.ClrType))
            {
                builder.Entity(entityType.ClrType).Property(nameof(IHasConcurrencyStamp.ConcurrencyStamp))
                    .IsConcurrencyToken();
            }
            
            if (typeof(ICreationAuditedObject).IsAssignableFrom(entityType.ClrType))
            {
                builder.Entity(entityType.ClrType).Property(nameof(ICreationAuditedObject.CreationTime))
                    .IsRequired();
                
                builder.Entity(entityType.ClrType).Property(nameof(ICreationAuditedObject.CreatorId))
                    .HasMaxLength(100);
            }
            
            if (typeof(IAuditedObject).IsAssignableFrom(entityType.ClrType))
            {
                builder.Entity(entityType.ClrType).Property(nameof(IAuditedObject.LastModificationTime))
                    .IsRequired(false);
                
                builder.Entity(entityType.ClrType).Property(nameof(IAuditedObject.LastModifierId))
                    .HasMaxLength(100).IsRequired(false);
            }
            
            if (typeof(IDeletionAuditedObject).IsAssignableFrom(entityType.ClrType))
            {
                builder.Entity(entityType.ClrType).Property(nameof(IDeletionAuditedObject.DeletionTime))
                    .IsRequired(false);
                
                builder.Entity(entityType.ClrType).Property(nameof(IDeletionAuditedObject.DeleterId))
                    .HasMaxLength(100).IsRequired(false);
            }
         
            var entityClrType = entityType.ClrType;
            var entityInterfaces = entityClrType.GetInterfaces();
            var isIEntity = entityInterfaces.Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEntity<>));

            if (isIEntity)
            {
                var idProperty = entityClrType.GetProperty("Id");
                if (idProperty != null)
                {
                    builder.Entity(entityClrType)
                        .Property(idProperty.Name)
                        .ValueGeneratedOnAdd();
                }
            }
        }

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public override int SaveChanges()
    {
        SetCreationTimestamps();
        SetModificationTimestamps();
        SetSoftDelete();
        DispatchDomainEvents().GetAwaiter();
        var result = base.SaveChanges();
        return result;
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SetCreationTimestamps();
        SetModificationTimestamps();
        SetSoftDelete();
        await DispatchDomainEvents();
        var result = await base.SaveChangesAsync(cancellationToken);
        return result;
    }

    #region Entity Creation Timestamps Methods

    private void SetCreationTimestamps()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is ICreationAuditedObject && e.State == EntityState.Added);

        foreach (var entry in entries)
        {
            var entity = (ICreationAuditedObject)entry.Entity;
            entity.SetCreationTime();
            entity.SetCreatorId(_httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }
    }

    #endregion

    #region Entity Update Timestamps Methods

    private void SetModificationTimestamps()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            if (entry.Entity is IAuditedObject)
            {
                var entity = (IAuditedObject)entry.Entity;
                entity.SetLastModificationTime();
                entity.SetLastModifierId(_httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            }

            if (entry.Entity is IHasConcurrencyStamp)
            {
                var entity = (IHasConcurrencyStamp)entry.Entity;
                entity.ConcurrencyStamp = Guid.NewGuid().ToString("N");
            }
        }
    }

    #endregion

    #region Entity Soft Delete Methods

    private void SetSoftDelete()
    {
        var entries = ChangeTracker.Entries()
            .Where(e =>
                e.Entity is IDeletionAuditedObject &&
                e.State == EntityState.Modified &&
                e.CurrentValues["IsDeleted"]!.Equals(true));

        foreach (var entry in entries)
        {
            var entity = (IDeletionAuditedObject)entry.Entity;
            entity.SetDeletionTime();
            entity.SetDeleterId(_httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }
    }

    #endregion
    
    private async Task DispatchDomainEvents()
    {
        var domainEntities = ChangeTracker.Entries<BasicAggregateRoot>()
            .Where(x => x.Entity.GetLocalEvents().Any() || x.Entity.GetDistributedEvents().Any())
            .ToList();

        foreach (var entityEntry in domainEntities)
        {
            var localDomainEvents = entityEntry.Entity.GetLocalEvents().ToList();
            var distributedDomainEvents = entityEntry.Entity.GetDistributedEvents().ToList();

            foreach (var domainEvent in localDomainEvents)
            {
                await PublishDomainEvent(domainEvent);
            }
            
            entityEntry.Entity.ClearLocalEvents();
            
            foreach (var domainEvent in distributedDomainEvents)
            {
                await PublishDomainEvent(domainEvent);
            }
            
            entityEntry.Entity.ClearDistributedEvents();
        }
    }
    
    protected virtual async Task PublishDomainEvent(DomainEventRecord domainEvent)
    {
        await _eventDispatcher.DispatchAsync(domainEvent.EventData);
    }
}