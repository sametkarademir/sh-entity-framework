using SoftwareHospital.EntityFramework.Core.Aggregates.CreationAuditedAggregateRoots;

namespace SoftwareHospital.EntityFramework.Core.Dtos;

public abstract class CreationAuditedEntityDto : EntityDto, ICreationAuditedObject
{
    public DateTime CreationTime { get; set; }
    public void SetCreationTime(DateTime? creationTime = null)
    {
        CreationTime = creationTime ?? DateTime.UtcNow;
    }
    
    public string? CreatorId { get; set; }
    public void SetCreatorId(string? creatorId)
    {
        CreatorId = creatorId;
    }
}

public abstract class CreationAuditedEntityDto<TKey> : EntityDto<TKey>, ICreationAuditedObject
{
    public DateTime CreationTime { get; set; }
    public string? CreatorId { get; set; }
    
    public void SetCreationTime(DateTime? creationTime = null)
    {
        CreationTime = creationTime ?? DateTime.UtcNow;
    }
    
    public void SetCreatorId(string? creatorId)
    {
        CreatorId = creatorId;
    }
}