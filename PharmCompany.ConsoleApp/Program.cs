using PharmCompany.ConsoleApp.Menu;
using System;

namespace PharmCompany.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            MenuManager.DisplayMenu();
        }
    }
}
