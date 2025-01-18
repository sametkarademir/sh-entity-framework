namespace SoftwareHospital.EntityFramework.Core.Aggregates.CreationAuditedAggregateRoots;

public interface IMayHaveCreator
{
    string? CreatorId { get;}
    
    public void SetCreatorId(string? creatorId);
}