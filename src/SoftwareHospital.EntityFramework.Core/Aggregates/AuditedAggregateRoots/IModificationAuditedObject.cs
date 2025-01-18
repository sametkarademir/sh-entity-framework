namespace SoftwareHospital.EntityFramework.Core.Aggregates.AuditedAggregateRoots;

public interface IModificationAuditedObject : IHasModificationTime
{
    string? LastModifierId { get; }
    
    public void SetLastModifierId(string? lastModifierId);
}