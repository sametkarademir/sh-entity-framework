using SoftwareHospital.EntityFramework.Core.Aggregates.AuditedAggregateRoots;

namespace SoftwareHospital.EntityFramework.Core.Dtos;

public class AuditedEntityDto : CreationAuditedEntityDto, IAuditedObject
{
    public DateTime? LastModificationTime { get; set; }
    public void SetLastModificationTime(DateTime? lastModificationTime = null)
    {
        LastModificationTime = lastModificationTime ?? DateTime.UtcNow;
    }

    public string? LastModifierId { get; set; }
    public void SetLastModifierId(string? lastModifierId)
    {
        LastModifierId = lastModifierId;
    }
}

public class AuditedEntityDto<TKey> : CreationAuditedEntityDto<TKey>, IAuditedObject
{
    public DateTime? LastModificationTime { get; set; }
    public void SetLastModificationTime(DateTime? lastModificationTime = null)
    {
        LastModificationTime = lastModificationTime ?? DateTime.UtcNow;
    }

    public string? LastModifierId { get; set; }
    public void SetLastModifierId(string? lastModifierId)
    {
        LastModifierId = lastModifierId;
    }
}