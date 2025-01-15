namespace SoftwareHospital.EntityFramework.Aggregates.CreationAuditedAggregateRoots;

public interface ICreationAuditedObject : IHasCreationTime, IMayHaveCreator
{
}