using PharmCompany.ConsoleApp.Entities.Operations;
using System;

namespace PharmCompany.ConsoleApp.DbLogics.Repositories
{
    public interface IRepository<T> 
        : IReadAllable<T>, ICreateableAsync<T>, IRemoveable<T>, IDisposable 
        where T : class
    {
    }
}
