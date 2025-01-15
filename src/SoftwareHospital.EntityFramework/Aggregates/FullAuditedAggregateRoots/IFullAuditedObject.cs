using SoftwareHospital.EntityFramework.Aggregates.AuditedAggregateRoots;
using SoftwareHospital.EntityFramework.Aggregates.CreationAuditedAggregateRoots;

namespace SoftwareHospital.EntityFramework.Aggregates.FullAuditedAggregateRoots;

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