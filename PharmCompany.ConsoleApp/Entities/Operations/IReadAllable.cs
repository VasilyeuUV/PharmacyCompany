using System.Collections.Generic;
using System.Threading.Tasks;

namespace PharmCompany.ConsoleApp.Entities.Operations
{
    public interface IReadAllable<T>
        where T : class
    {
        /// <summary>
        /// Получение всех сущностей в асинхронном режиме
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> GetAllAsync();
    }
}
