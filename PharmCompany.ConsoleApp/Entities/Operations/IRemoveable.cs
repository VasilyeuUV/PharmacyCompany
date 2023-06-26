using PharmCompany.Entities.Capabilities;
using System;

namespace PharmCompany.ConsoleApp.Entities.Operations
{
    /// <summary>
    /// Способный удалять
    /// </summary>
    public interface IRemoveable<T>
        where T : class
    {
        /// <summary>
        /// Удаление сущности по id
        /// </summary>
        /// <param name="id">id ceoyjcnb</param>
        /// <returns>true/false успешное завершение операции</returns>
        bool Remove(Guid id);
    }
}
