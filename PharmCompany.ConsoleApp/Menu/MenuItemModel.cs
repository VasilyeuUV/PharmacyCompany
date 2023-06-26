using PharmCompany.ConsoleApp.Models.DbTables;
using System;

namespace PharmCompany.ConsoleApp.Menu
{
    /// <summary>
    /// Модель пункта меню
    /// </summary>
    internal class MenuItemModel
    {

        /// <summary>
        /// Наименование пункта меню
        /// </summary>
        public string MenuItemName { get; set; }


        /// <summary>
        /// Действие пункта меню
        /// </summary>
        public Action MenuItemAction { get; set; }


        /// <summary>
        /// Наименование таблицы в БД.
        /// </summary>
        public DbTableModel DbTable { get; set; }

        /// <summary>
        /// Подменю
        /// </summary>
        public MenuItemModel[] SubMenu { get; set; }
    }
}
