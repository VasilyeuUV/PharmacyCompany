using PharmCompany.Entities.Capabilities;
using System.Threading.Tasks;

namespace PharmCompany.ConsoleApp.Entities.Operations
{
    /// <summary>
    /// Способный создавать
    /// </summary>
    public interface ICreateableAsync<T>
        where T : class
    {
        /// <summary>
        /// Создание (добавление) сущности в асинхронном режиме
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task<T> AddAsync(T item);
    }
}
