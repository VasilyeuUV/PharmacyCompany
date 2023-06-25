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
        public PharmacyModel Pharmacy { get; set; }
    }
}
