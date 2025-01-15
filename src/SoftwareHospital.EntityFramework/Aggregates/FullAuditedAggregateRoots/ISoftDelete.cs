namespace SoftwareHospital.EntityFramework.Aggregates.FullAuditedAggregateRoots;

public interface ISoftDelete
{
    bool IsDeleted { get; set; }
}