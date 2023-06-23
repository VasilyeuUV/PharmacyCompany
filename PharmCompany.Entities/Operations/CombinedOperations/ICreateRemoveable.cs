using PharmCompany.Entities.Capabilities;

namespace PharmCompany.Entities.Operations.CombinedOperations
{
    public interface ICreateRemoveable<T> : ICreateableAsync<T>, IRemoveable<T>
        where T : class, IIdentifiable
    {
    }
}
