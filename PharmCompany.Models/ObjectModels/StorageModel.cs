using PharmCompany.Entities.BaseEntities;

namespace PharmCompany.Models.ObjectModels
{
    /// <summary>
    /// Склад аптеки
    /// </summary>
    public class StorageModel : ANameableEntityBase
    {

        //#################################################################################################################
        #region CTOR

        public StorageModel(string name) : base(name)
        {
        }

        public StorageModel(string name, Guid guid) : base(name, guid)
        {
        }

        #endregion // CTOR


        /// <summary>
        ///  Аптека
        /// </summary>
        public PharmacyModel Pharmacy { get; set; }
    }
}
