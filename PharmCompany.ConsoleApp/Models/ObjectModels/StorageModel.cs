using System;
using System.ComponentModel;
using System.Security.Policy;

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
        //public PharmacyModel PharmacyModel { get; set; }


        /// <summary>
        ///  Guid Аптеки
        /// </summary>
        [DisplayName("Номер Аптеки")]
        public Guid PharmacyId { get; set; }


        //public override string DisplayFormat => $"{Name} ({PharmacyModel?.Name}, {PharmacyModel?.Adress})";
    }
}
