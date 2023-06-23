namespace PharmCompany.ConsoleApp.Menu
{
    /// <summary>
    /// Навигация по меню 
    /// </summary>
    internal class MenuNavigator
    {
        private static int counter = 0;

        private string[] _menuItems;        // - пункты текущего меню


        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name=""></param>
        public MenuNavigator(string[] menuItems)
            => this._menuItems = menuItems;



        /// <summary>
        /// Выбор пункта меню
        /// </summary>
        /// <returns>selected Menu item</returns>
        public int SelectMenu(string op)
        {
            ConsoleKeyInfo key;
            do
            {
                Console.Clear();

                Console.WriteLine(op);
                Console.WriteLine();

                // - MENU
                if (counter >= _menuItems.Length)
                    counter = _menuItems.Length - 1;

                for (int i = 0; i < _menuItems.Length; i++)
                {
                    if (counter == i)
                    {
                        Console.BackgroundColor = ConsoleColor.Cyan;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.WriteLine(_menuItems[i]);
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                        Console.WriteLine(_menuItems[i]);
                }

                key = Console.ReadKey();
                counter = key.Key switch
                {
                    ConsoleKey.UpArrow => --counter == -1
                        ? _menuItems.Length - 1
                        : _menuItems.Length,
                    ConsoleKey.DownArrow => ++counter == _menuItems.Length
                        ? 0
                        : _menuItems.Length,
                    ConsoleKey.Escape => _menuItems.Length - 1,
                    _ => _menuItems.Length
                };
            }
            while (key.Key != ConsoleKey.Enter);

            return counter;
        }


    }
}
