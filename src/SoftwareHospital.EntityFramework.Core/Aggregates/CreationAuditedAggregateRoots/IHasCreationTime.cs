namespace SoftwareHospital.EntityFramework.Core.Aggregates.CreationAuditedAggregateRoots;

public interface IHasCreationTime
{
    DateTime CreationTime { get; }
    public void SetCreationTime(DateTime? creationTime = null);
}