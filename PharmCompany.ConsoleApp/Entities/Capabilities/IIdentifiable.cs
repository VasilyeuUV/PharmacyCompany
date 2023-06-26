using System;

namespace PharmCompany.Entities.Capabilities
{
    /// <summary>
    /// Идентифицируемый
    /// </summary>
    public interface IIdentifiable
    {
        /// <summary>
        /// Идентфиикатор
        /// </summary>
        Guid Id { get; }
    }
}
