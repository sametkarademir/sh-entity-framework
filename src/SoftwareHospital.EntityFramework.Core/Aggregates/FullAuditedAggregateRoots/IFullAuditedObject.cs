using SoftwareHospital.EntityFramework.Core.Aggregates.AuditedAggregateRoots;
using SoftwareHospital.EntityFramework.Core.Aggregates.CreationAuditedAggregateRoots;

namespace SoftwareHospital.EntityFramework.Core.Aggregates.FullAuditedAggregateRoots;

public interface IFullAuditedObject : 
    IAuditedObject,
    ICreationAuditedObject,
    IHasCreationTime,
    IMayHaveCreator,
    IModificationAuditedObject,
    IHasModificationTime,
    IDeletionAuditedObject,
    IHasDeletionTime,
    ISoftDelete
{
}