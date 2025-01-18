using SoftwareHospital.EntityFramework.Core.Aggregates.CreationAuditedAggregateRoots;

namespace SoftwareHospital.EntityFramework.Core.Aggregates.AuditedAggregateRoots;

public interface IAuditedObject : 
    ICreationAuditedObject,
    IHasCreationTime,
    IMayHaveCreator,
    IModificationAuditedObject,
    IHasModificationTime
{
}