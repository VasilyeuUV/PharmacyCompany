using System.ComponentModel;

namespace PharmCompany.ConsoleApp.Models
{
    /// <summary>
    /// Склад аптеки
    /// </summary>
    public class StorageModel : ANameableEntityBase
    {
        /// <summary>
        ///  Аптека
        /// </summary>
        [DisplayName("Номер Аптеки")] 
        public PharmacyModel Pharmacy { get; set; }
    }
}
