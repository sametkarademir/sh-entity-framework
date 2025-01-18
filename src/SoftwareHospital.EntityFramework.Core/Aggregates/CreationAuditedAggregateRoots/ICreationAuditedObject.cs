namespace SoftwareHospital.EntityFramework.Core.Aggregates.CreationAuditedAggregateRoots;

public interface ICreationAuditedObject : IHasCreationTime, IMayHaveCreator
{
}