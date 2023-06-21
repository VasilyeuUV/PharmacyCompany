using PharmCompany.Entities.BaseEntities;

namespace PharmCompany.Models.ObjectModels
{
    public class PharmacyModel : ANameableEntityBase
    {

        //#################################################################################################################
        #region CTOR
        public PharmacyModel(string name) : base(name)
        {
        }

        public PharmacyModel(string name, Guid guid) : base(name, guid)
        {
        }

        #endregion // CTOR


        /// <summary>
        /// Адрес
        /// </summary>
        public string? Adress { get; set; }


        /// <summary>
        /// Телефрн
        /// </summary>
        public string? Phone { get; set; }
    }
}
