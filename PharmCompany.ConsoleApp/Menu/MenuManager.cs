﻿using PharmCompany.ConsoleApp.Services;
using System;

namespace PharmCompany.ConsoleApp.Menu
{
    /// <summary>
    /// Мэнеджер меню
    /// </summary>
    internal static class MenuManager
    {
        private static readonly string _COMPANY_NAME = strings.CompanyName;
        private static MenuItemModel _selectedMenuItem;
        private static MenuItemModel _selectedMainMenuItem;

        // - главное меню
        private static MenuItemModel[] _mainMenu = {
            new MenuItemModel {MenuItemName = strings.Goods, MenuItemAction = DisplayOperationMenu},
            new MenuItemModel {MenuItemName = strings.Pharmacy, MenuItemAction = DisplayOperationMenu},
            new MenuItemModel {MenuItemName = strings.Storages, MenuItemAction = DisplayOperationMenu},
            new MenuItemModel {MenuItemName = strings.BatchGoods, MenuItemAction = DisplayOperationMenu},
            new MenuItemModel {MenuItemName = strings.Exit, MenuItemAction = Exit},
        };

        // - меню с общими операциями 
        private static MenuItemModel[] _commonOperationMenu ={
            new MenuItemModel {MenuItemName = "Показать список", MenuItemAction = DisplayContent},
            new MenuItemModel {MenuItemName = strings.Create, MenuItemAction = CreateEntity},
            new MenuItemModel {MenuItemName = strings.Remove, MenuItemAction = RemoveEntity},
            new MenuItemModel {MenuItemName = strings.Back, MenuItemAction = Back},
        };


        private static void DisplayContent()
        {
            throw new NotImplementedException();
        }


        //#######################################################################################################################
        #region Menu actions

        /// <summary>
        /// Показать меню с задачами
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private static void DisplayOperationMenu()
        {
            _selectedMainMenuItem = _selectedMenuItem;
            DisplayMenu(_selectedMenuItem?.MenuItemName, _commonOperationMenu);
        }


        /// <summary>
        /// Создать сущность
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private static void CreateEntity()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Удалить сущность
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private static void RemoveEntity()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Вернуться в главное меню
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private static void Back()
        {
            DisplayMenu();
        }


        /// <summary>
        /// Выход из текущего меню
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private static void Exit()
        {
            DisplayToConsole.WaitForContinue("Work completed.");
        }

        #endregion // Menu actions


        /// <summary>
        /// 
        /// </summary>
        internal static void DisplayMenu(string menuTitle = null, MenuItemModel[] menu = null)
        {
            if (string.IsNullOrEmpty(menuTitle))
            {
                menuTitle = _COMPANY_NAME;
                menu = _mainMenu;
                _selectedMainMenuItem = null;
            }
            else if (menu == null)
                throw new NullReferenceException("No menu specified");

            _selectedMenuItem = SelectMenuItem(menuTitle, menu);
            _selectedMenuItem?.MenuItemAction?.Invoke();
        }


        private static MenuItemModel SelectMenuItem(string menuTitle, MenuItemModel[] menu)
        {
            int counter = 0;
            var selectedMenu = menu[counter];
            ConsoleKeyInfo key;

            do
            {
                DisplayToConsole.DisplayTitle(menuTitle, true);

                DisplayToConsole.DisplayMenu(menuTitle, menu, selectedMenu);

                key = Console.ReadKey();
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (--counter < 0)
                            counter = menu.Length - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        if (++counter > menu.Length - 1)
                            counter = 0;
                        break;
                    case ConsoleKey.Escape:
                        counter = menu.Length - 1;
                        break;
                }
                selectedMenu = menu[counter];
            }
            while (key.Key != ConsoleKey.Enter);

            return selectedMenu;
        }
    }
}