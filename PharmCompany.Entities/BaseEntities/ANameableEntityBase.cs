using PharmCompany.Entities.Capabilities;

namespace PharmCompany.Entities.BaseEntities
{
    /// <summary>
    /// Базовая именованная сущность
    /// </summary>
    public abstract class ANameableEntityBase : IIdentifiable, INameable
    {

        /// <summary>
        /// CTOR
        /// </summary>
        protected ANameableEntityBase(string name, Guid guid)
        {

            this.Name = name;
            this.Id = guid == Guid.Empty
                ? Guid.NewGuid()
                : guid;
        }


        public Guid Id { get; private set; }
        public string Name { get; private set; }

    }
}
