using PharmCompany.Entities.Capabilities;

namespace PharmCompany.Entities.Operations
{
    /// <summary>
    /// Способный создавать
    /// </summary>
    public interface ICreateableAsync<T>
        where T : class, IIdentifiable
    {
        /// <summary>
        /// Создание (добавление) сущности в асинхронном режиме
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task<T> AddAsync(T item);
    }
}
