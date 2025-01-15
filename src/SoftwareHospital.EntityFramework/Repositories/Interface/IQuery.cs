namespace SoftwareHospital.EntityFramework.Repositories.Interface;

public interface IQuery<T>
{
    IQueryable<T> Query();
}