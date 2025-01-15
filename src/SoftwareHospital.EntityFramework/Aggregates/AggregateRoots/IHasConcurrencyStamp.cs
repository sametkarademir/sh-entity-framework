namespace SoftwareHospital.EntityFramework.Aggregates.AggregateRoots;

public interface IHasConcurrencyStamp
{
    string ConcurrencyStamp { get; set; }
}