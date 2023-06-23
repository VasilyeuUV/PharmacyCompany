using PharmCompany.ConsoleApp.Menu;
using System;

namespace PharmCompany.ConsoleApp.Services
{

    /// <summary>
    /// Вывести на консоль
    /// </summary>
    internal static class DisplayToConsole
    {
        /// <summary>
        /// Display Title
        /// </summary>
        /// <param name="title">text to view</param>
        /// <param name="isClearConsole">true if Console clear</param>
        internal static void DisplayTitle(string title, bool isClearConsole = false)
        {
            var foregraundColor = isClearConsole
                ? ConsoleColor.Green
                : ConsoleColor.Yellow;

            if (isClearConsole)
                Console.Clear();

            Console.ForegroundColor = foregraundColor;
            Console.WriteLine(title + ":");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
        }


        /// <summary>
        /// Display Body
        /// </summary>
        /// <param name="title"></param>
        internal static void DisplayBody(string[] strigs)
        {
            foreach (var str in strigs)
                Console.WriteLine(str);
            Console.WriteLine();
        }


        /// <summary>
        /// Show information if there are many objects
        /// </summary>
        internal static void DisplayInfo(string v, int objCount, int viewCount = 0)
        {
            if (viewCount == 0)
                viewCount = objCount;

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"(Shown {viewCount} from {objCount} {v})");
            Console.ForegroundColor = ConsoleColor.White;
        }


        /// <summary>
        /// Wait push key 
        /// </summary>
        /// <param name="str"></param>
        internal static ConsoleKeyInfo WaitForContinue(string str = "")
        {
            if (!string.IsNullOrEmpty(str.Trim()))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(str);
                Console.ForegroundColor = ConsoleColor.White;
            }
            Console.WriteLine();
            Console.WriteLine("Press key to continue");
            return Console.ReadKey();
        }


        /// <summary>
        /// Отобразить меню в консоли
        /// </summary>
        /// <param name="menuTitle"></param>
        /// <param name="menu"></param>
        /// <param name="selectedMenu"></param>
        /// <exception cref="NotImplementedException"></exception>
        internal static void DisplayMenu(string menuTitle, MenuItemModel[] menu, MenuItemModel selectedMenu)
        {
            for (int i = 0; i < menu.Length; i++)
            {
                var isSelectedMenuItem = selectedMenu == menu[i];

                Console.BackgroundColor = isSelectedMenuItem
                    ? ConsoleColor.Cyan
                    : ConsoleColor.Black;
                Console.ForegroundColor = isSelectedMenuItem
                    ? ConsoleColor.Black
                    : ConsoleColor.White;

                Console.WriteLine($"{i + 1}. {menu[i].MenuItemName} ");
            }
            Console.WriteLine();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
