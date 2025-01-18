namespace SoftwareHospital.EntityFramework.Core.Aggregates.FullAuditedAggregateRoots;

public interface ISoftDelete
{
    bool IsDeleted { get; set; }
}