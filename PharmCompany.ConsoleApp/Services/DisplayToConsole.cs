using PharmCompany.ConsoleApp.Menu;
using PharmCompany.ConsoleApp.Models;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

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

        internal static T CreateObject<T>()
            where T : new()
        {
            T entity = new T();
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo property in properties)
            {
                if (property.Name == "Id")
                    property.SetValue(entity, Guid.NewGuid());
                else
                    property.SetValue(entity, DisplayToConsole.InputValue(property));
            }

            return entity;
        }

        private static string InputValue(PropertyInfo property)
        {
            Console.Write($"{strings.Input} {GetAttributeDisplayName(property)}:");
            string value = Console.ReadLine();
            Console.WriteLine();
            return value;
        }


        private static string GetAttributeDisplayName(PropertyInfo property)
        {
            var atts = property.GetCustomAttributes(typeof(DisplayNameAttribute), true);
            if (atts.Length == 0)
                return null;
            return (atts[0] as DisplayNameAttribute).DisplayName;
        }



        //public static string GetPropertyDisplayName<T>(Expression<Func<T, object>> propertyExpression)
        //{
        //    var memberInfo = GetPropertyInformation(propertyExpression.Body);
        //    if (memberInfo == null)
        //    {
        //        throw new ArgumentException(
        //            "No property reference expression was found.",
        //            "propertyExpression");
        //    }

        //    var attr = memberInfo.GetAttribute<DisplayNameAttribute>(false);
        //    if (attr == null)
        //    {
        //        return memberInfo.Name;
        //    }

        //    return attr.DisplayName;
        //}


        //public static MemberInfo GetPropertyInformation(Expression propertyExpression)
        //{
        //    Debug.Assert(propertyExpression != null, "propertyExpression != null");
        //    MemberExpression memberExpr = propertyExpression as MemberExpression;
        //    if (memberExpr == null)
        //    {
        //        UnaryExpression unaryExpr = propertyExpression as UnaryExpression;
        //        if (unaryExpr != null
        //            && unaryExpr.NodeType == ExpressionType.Convert
        //            )
        //            memberExpr = unaryExpr.Operand as MemberExpression;

        //    }

        //    if (memberExpr != null
        //        && memberExpr.Member.MemberType == MemberTypes.Property
        //        )
        //        return memberExpr.Member;

        //    return null;
        //}
    }
}
