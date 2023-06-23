namespace PharmCompany.ConsoleUI.Menu
{
    /// <summary>
    /// Модель пункта меню
    /// </summary>
    internal class MenuItemModel
    {
        /// <summary>
        /// Наименование пункта меню
        /// </summary>
        public string? MenuItemName { get; set; }


        /// <summary>
        /// Действие пункта меню
        /// </summary>
        public Action? MenuItemAction { get; set; }
    }
}
