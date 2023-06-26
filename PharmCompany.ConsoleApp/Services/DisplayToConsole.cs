using PharmCompany.ConsoleApp.Menu;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Text;

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
        internal static void DisplayBody(string[] items, bool isNumberedList = false)
        {
            for (int i = 0; i < items.Length; i++)
            {
                var item = isNumberedList
                    ? $"{i + 1}. {items[i]}."
                    : $"{items[i]}";
                Console.WriteLine(item);
            }

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


        internal static T CreateObject<T>()
            where T : new()
        {
            T entity = new T();
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo property in properties)
            {
                if (property.Name == "Id")
                    property.SetValue(entity, Guid.NewGuid());
                else if (property.Name.EndsWith("Id"))
                    continue;
                else if (property.PropertyType == typeof(int))
                {
                    var value = DisplayToConsole.InputIntValue(GetAttributeDisplayName(property));
                    if (value != null)
                        property.SetValue(entity, value);
                }
                else
                {
                    var value = DisplayToConsole.InputValue(property);
                    if (value != null)
                        property.SetValue(entity, value);
                }
            }

            return entity;
        }


        private static string InputValue(PropertyInfo property)
        {
            var displayName = GetAttributeDisplayName(property);
            if (string.IsNullOrEmpty(displayName))
                return null;

            Console.Write($"{strings.Input} {displayName}: ");
            string value = Console.ReadLine();
            Console.WriteLine();
            return value;
        }


        internal static int? InputIntValue(string welcomeTxt)
        {            
            Console.Write($"{welcomeTxt}: " );

            ConsoleKeyInfo key;
            var sb = new StringBuilder();

            while (true)
            {
                key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Escape)
                    return null;
                if (key.Key == ConsoleKey.Enter)
                    break;

                if (char.IsDigit(key.KeyChar))
                {
                    sb.Append(key.KeyChar);
                    Console.Write(key.KeyChar);
                }
            }
            Console.WriteLine();

            int result = 0;
            int.TryParse(sb.ToString(), out result);
            return result;
        }


        private static string GetAttributeDisplayName(PropertyInfo property)
        {
            var atts = property.GetCustomAttributes(typeof(DisplayNameAttribute), true);
            if (atts.Length == 0)
                return null;
            return (atts[0] as DisplayNameAttribute).DisplayName;
        }
    }
}
