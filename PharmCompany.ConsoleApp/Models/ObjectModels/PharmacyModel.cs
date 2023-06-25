using System;
using System.ComponentModel;

namespace PharmCompany.ConsoleApp.Models
{
    public class PharmacyModel : ANameableEntityBase
    {
        /// <summary>
        /// Адрес
        /// </summary>
        [DisplayName("Адрес")] 
        public string Adress { get; set; }


        /// <summary>
        /// Телефрн
        /// </summary>
        [DisplayName("Телефон")]
        public string Phone { get; set; }
    }
}
