using PharmCompany.Entities.Capabilities;

namespace PharmCompany.Entities.BaseEntities
{
    /// <summary>
    /// Базовая именованная сущность
    /// </summary>
    public abstract class ANameableEntityBase : IIdentifiable, INameable
    {

        //#################################################################################################################
        #region CTOR

        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="name"></param>
        /// <param name="guid"></param>
        protected ANameableEntityBase(string name, Guid guid)
        {

            this.Name = name;
            this.Id = guid == Guid.Empty
                ? Guid.NewGuid()
                : guid;
        }

        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="name"></param>
        protected ANameableEntityBase(string name) : this(name, Guid.Empty) { }

        #endregion // CTOR


        public Guid Id { get; private set; }
        public string Name { get; private set; }

    }
}
