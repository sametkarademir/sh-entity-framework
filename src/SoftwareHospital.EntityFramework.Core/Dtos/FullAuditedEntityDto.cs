using SoftwareHospital.EntityFramework.Core.Aggregates.FullAuditedAggregateRoots;

namespace SoftwareHospital.EntityFramework.Core.Dtos;

public class FullAuditedEntityDto : AuditedEntityDto, IFullAuditedObject
{
    public bool IsDeleted { get; set; }
    public DateTime? DeletionTime { get; set; }
    public void SetDeletionTime(DateTime? deletionTime = null)
    {
        DeletionTime = deletionTime ?? DateTime.UtcNow;
    }

    public string? DeleterId { get; set; }
    public void SetDeleterId(string? deleterId)
    {
        DeleterId = deleterId;
    }
}

public class FullAuditedEntityDto<TKey> : AuditedEntityDto<TKey>, IFullAuditedObject
{
    public bool IsDeleted { get; set; }
    public DateTime? DeletionTime { get; set; }
    public void SetDeletionTime(DateTime? deletionTime = null)
    {
        DeletionTime = deletionTime ?? DateTime.UtcNow;
    }

    public string? DeleterId { get; set; }
    public void SetDeleterId(string? deleterId)
    {
        DeleterId = deleterId;
    }
}