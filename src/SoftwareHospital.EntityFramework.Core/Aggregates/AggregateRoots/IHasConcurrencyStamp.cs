namespace SoftwareHospital.EntityFramework.Core.Aggregates.AggregateRoots;

public interface IHasConcurrencyStamp
{
    string ConcurrencyStamp { get; set; }
}