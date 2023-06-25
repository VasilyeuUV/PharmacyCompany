using System;
using System.ComponentModel;

namespace PharmCompany.ConsoleApp.Models
{
    /// <summary>
    /// Базовая именованная сущность
    /// </summary>
    public abstract class ANameableEntityBase
    {
        [DisplayName("Идентификатор")]
        public Guid Id { get; internal set; }


        [DisplayName("Наименование")]
        public string Name { get; internal set; }

    }
}
