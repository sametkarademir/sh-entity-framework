namespace SoftwareHospital.EntityFramework.Aggregates.FullAuditedAggregateRoots;

public interface IHasDeletionTime : ISoftDelete
{
    DateTime? DeletionTime { get; }
    
    public void SetDeletionTime(DateTime? deletionTime = null);
}