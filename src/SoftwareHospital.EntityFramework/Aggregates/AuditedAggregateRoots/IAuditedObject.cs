using SoftwareHospital.EntityFramework.Aggregates.CreationAuditedAggregateRoots;

namespace SoftwareHospital.EntityFramework.Aggregates.AuditedAggregateRoots;

public interface IAuditedObject : 
    ICreationAuditedObject,
    IHasCreationTime,
    IMayHaveCreator,
    IModificationAuditedObject,
    IHasModificationTime
{
}