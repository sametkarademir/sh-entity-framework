using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Options;
using SoftwareHospital.EntityFramework.Core.Aggregates.AggregateRoots;
using SoftwareHospital.EntityFramework.Core.Aggregates.AuditedAggregateRoots;
using SoftwareHospital.EntityFramework.Core.Aggregates.CreationAuditedAggregateRoots;
using SoftwareHospital.EntityFramework.Core.Aggregates.Entities;
using SoftwareHospital.EntityFramework.Core.Aggregates.FullAuditedAggregateRoots;
using SoftwareHospital.EntityFramework.Enums;
using SoftwareHospital.EntityFramework.Extensions;
using SoftwareHospital.EntityFramework.Models;

namespace SoftwareHospital.EntityFramework.Contexts;

public abstract class BaseDbContext : DbContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IEventDispatcher _eventDispatcher;
    private readonly EntityFrameworkSettings _settings;

    protected BaseDbContext(DbContextOptions options, IHttpContextAccessor httpContextAccessor, IEventDispatcher eventDispatcher, IOptions<EntityFrameworkSettings> settings) : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
        _eventDispatcher = eventDispatcher;
        _settings = settings.Value;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        foreach (var entityType in builder.Model.GetEntityTypes())
        {
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
                builder.Entity(entityType.ClrType)
                    .Property(nameof(IHasConcurrencyStamp.ConcurrencyStamp))
                    .HasMaxLength(256)
                    .IsRequired()
                    .IsConcurrencyToken();
            }
            
            if (typeof(ICreationAuditedObject).IsAssignableFrom(entityType.ClrType))
            {
                builder.Entity(entityType.ClrType).Property
                        (nameof(ICreationAuditedObject.CreationTime))
                    .IsRequired();
                
                builder.Entity(entityType.ClrType)
                    .Property(nameof(ICreationAuditedObject.CreatorId))
                    .HasMaxLength(256)
                    .IsRequired(false);
            }
            
            if (typeof(IAuditedObject).IsAssignableFrom(entityType.ClrType))
            {
                builder.Entity(entityType.ClrType)
                    .Property(nameof(IAuditedObject.LastModificationTime))
                    .IsRequired(false);
                
                builder.Entity(entityType.ClrType)
                    .Property(nameof(IAuditedObject.LastModifierId))
                    .HasMaxLength(256)
                    .IsRequired(false);
            }
            
            if (typeof(IDeletionAuditedObject).IsAssignableFrom(entityType.ClrType))
            {
                builder.Entity(entityType.ClrType)
                    .Property(nameof(IDeletionAuditedObject.DeletionTime))
                    .IsRequired(false);
                
                builder.Entity(entityType.ClrType)
                    .Property(nameof(IDeletionAuditedObject.DeleterId))
                    .HasMaxLength(256)
                    .IsRequired(false);
            }
            
            if (typeof(IHasExtraProperties).IsAssignableFrom(entityType.ClrType))
            {
                var comparer = new ValueComparer<ExtraPropertyDictionary>(
                    (c1, c2) => c1.SequenceEqual(c2), 
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => new ExtraPropertyDictionary(c));
                
                builder.Entity(entityType.ClrType)
                    .Property(nameof(IHasExtraProperties.ExtraProperties))
                    .HasConversion(new ExtraPropertyDictionaryConverter())
                    .Metadata.SetValueComparer(comparer);
                
                switch (_settings.DatabaseProviderType)
                {
                    case DatabaseProviderTypes.SqlServer:
                        builder.Entity(entityType.ClrType)
                            .Property(nameof(IHasExtraProperties.ExtraProperties))
                            .HasColumnType("nvarchar(max)");
                        break;
                    case DatabaseProviderTypes.PostgreSQL:
                        builder.Entity(entityType.ClrType)
                            .Property(nameof(IHasExtraProperties.ExtraProperties))
                            .HasColumnType("json");
                        break;
                    case DatabaseProviderTypes.MySQL:
                        builder.Entity(entityType.ClrType)
                            .Property(nameof(IHasExtraProperties.ExtraProperties))
                            .HasColumnType("json");
                        break;
                    case DatabaseProviderTypes.SQLite:
                        builder.Entity(entityType.ClrType)
                            .Property(nameof(IHasExtraProperties.ExtraProperties))
                            .HasColumnType("text");
                        break;
                    case DatabaseProviderTypes.Oracle:
                        builder.Entity(entityType.ClrType)
                            .Property(nameof(IHasExtraProperties.ExtraProperties))
                            .HasColumnType("text");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }

    public override int SaveChanges()
    {
        this.SetCreationTimestamps(_httpContextAccessor);
        this.SetModificationTimestamps(_httpContextAccessor);
        this.SetSoftDelete(_httpContextAccessor);
        this.DispatchDomainEvents(_eventDispatcher).GetAwaiter();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        this.SetCreationTimestamps(_httpContextAccessor);
        this.SetModificationTimestamps(_httpContextAccessor);
        this.SetSoftDelete(_httpContextAccessor);
        await this.DispatchDomainEvents(_eventDispatcher);
        return await base.SaveChangesAsync(cancellationToken);
    }
}