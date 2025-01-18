namespace SoftwareHospital.EntityFramework.Core.Aggregates.FullAuditedAggregateRoots;

public interface IDeletionAuditedObject : IHasDeletionTime, ISoftDelete
{
    string? DeleterId { get; }
    
    public void SetDeleterId(string? deleterId);
}