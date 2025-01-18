namespace SoftwareHospital.EntityFramework.Core.Aggregates.AuditedAggregateRoots;

public interface IHasModificationTime
{
    DateTime? LastModificationTime { get; }
    
    public void SetLastModificationTime(DateTime? lastModificationTime = null);
}