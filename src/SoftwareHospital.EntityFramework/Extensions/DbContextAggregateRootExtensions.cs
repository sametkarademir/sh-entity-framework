using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SoftwareHospital.EntityFramework.Core.Aggregates.AggregateRoots;
using SoftwareHospital.EntityFramework.Core.Aggregates.AuditedAggregateRoots;
using SoftwareHospital.EntityFramework.Core.Aggregates.BasicAggregateRoots;
using SoftwareHospital.EntityFramework.Core.Aggregates.CreationAuditedAggregateRoots;
using SoftwareHospital.EntityFramework.Core.Aggregates.FullAuditedAggregateRoots;

namespace SoftwareHospital.EntityFramework.Extensions;

public static class DbContextAggregateRootExtensions
{
    public static void SetCreationTimestamps(this DbContext context, IHttpContextAccessor httpContextAccessor)
    {
        var entries = context.ChangeTracker.Entries()
            .Where(e => e.Entity is ICreationAuditedObject && e.State == EntityState.Added);

        foreach (var entry in entries)
        {
            var entity = (ICreationAuditedObject)entry.Entity;
            entity.SetCreationTime();
            entity.SetCreatorId(httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }
    }
    
    public static void SetModificationTimestamps(this DbContext context, IHttpContextAccessor httpContextAccessor)
    {
        var entries = context.ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            if (entry.Entity is IAuditedObject)
            {
                var entity = (IAuditedObject)entry.Entity;
                entity.SetLastModificationTime();
                entity.SetLastModifierId(httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            }

            if (entry.Entity is IHasConcurrencyStamp)
            {
                var entity = (IHasConcurrencyStamp)entry.Entity;
                entity.ConcurrencyStamp = Guid.NewGuid().ToString("N");
            }
        }
    }
    
    public static void SetSoftDelete(this DbContext context, IHttpContextAccessor httpContextAccessor)
    {
        var entries = context.ChangeTracker.Entries()
            .Where(e =>
                e.Entity is IDeletionAuditedObject &&
                e.State == EntityState.Modified &&
                e.CurrentValues["IsDeleted"]!.Equals(true));

        foreach (var entry in entries)
        {
            var entity = (IDeletionAuditedObject)entry.Entity;
            entity.SetDeletionTime();
            entity.SetDeleterId(httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }
    }
    
    public static async Task DispatchDomainEvents(this DbContext context, IEventDispatcher eventDispatcher)
    {
        var domainEntities = context.ChangeTracker.Entries()
            .Where<EntityEntry>((Func<EntityEntry, bool>)(e => e.Entity is IGeneratesDomainEvents))
            .ToList();
        
        foreach (var entityEntry in domainEntities)
        {
            IGeneratesDomainEvents entity = (IGeneratesDomainEvents)entityEntry.Entity;
            foreach (DomainEventRecord domainEvent in entity.GetLocalEvents())
            {
                await eventDispatcher.DispatchAsync(domainEvent.EventData);
            }
            foreach (DomainEventRecord domainEvent in entity.GetDistributedEvents())
            {
                await eventDispatcher.DispatchAsync(domainEvent.EventData);
            }
            entity.ClearLocalEvents();
            entity.ClearDistributedEvents();
        }
    }
}