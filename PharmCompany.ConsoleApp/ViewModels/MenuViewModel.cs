using PharmCompany.ConsoleApp.Menu;
using System.Collections.Generic;

namespace PharmCompany.ConsoleApp.ViewModels
{
    /// <summary>
    /// Меню приложения
    /// </summary>
    internal class MenuViewModel
    {
        public IEnumerable<MenuItemModel> MainMenu { get; set; }
        public IEnumerable<MenuItemModel> OperationMenu { get; set; }
    }
}
