using PharmCompany.ConsoleApp.DbLogics;
using PharmCompany.ConsoleApp.Menu.MenuCommands;
using PharmCompany.ConsoleApp.Models;
using PharmCompany.ConsoleApp.Services;
using PharmCompany.ConsoleApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

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
        private static DbCommands _dbCommands;
        private static MainViewModel _mainViewModel = new MainViewModel();


        // - главное меню
        private static MenuItemModel[] _mainMenu = {
            new MenuItemModel
            {
                MenuItemName = strings.Goods,
                MenuItemAction = DisplayOperationMenu,
                DbTable = DbCommands.DbTables.FirstOrDefault(table => table.TableName.StartsWith("Goods")),
                SubMenu = new[] {
                    new MenuItemModel {MenuItemName = strings.GetGoods, MenuItemAction = SelectCommands.GetGoods}, 
                    new MenuItemModel {MenuItemName = strings.Create, MenuItemAction = InsertCommands.CreateEntity<GoodsModel>},
                    new MenuItemModel {MenuItemName = strings.Remove, MenuItemAction = DeleteCommands.RemoveEntity<GoodsModel>},
                    new MenuItemModel {MenuItemName = strings.Back, MenuItemAction = Back},
                }
            },
            new MenuItemModel
            {
                MenuItemName = strings.Pharmacy,
                MenuItemAction = DisplayOperationMenu,
                DbTable = DbCommands.DbTables.FirstOrDefault(table => table.TableName.StartsWith("Pharmacies")),
                SubMenu = new[] {
                    new MenuItemModel {MenuItemName = strings.Create, MenuItemAction = InsertCommands.CreateEntity<PharmacyModel>},
                    new MenuItemModel {MenuItemName = strings.Remove, MenuItemAction = DeleteCommands.RemoveEntity<PharmacyModel>},
                    new MenuItemModel {MenuItemName = strings.Back, MenuItemAction = Back},
                }
            },
            new MenuItemModel
            {
                MenuItemName = strings.Storages,
                MenuItemAction = DisplayOperationMenu,
                DbTable = DbCommands.DbTables.FirstOrDefault(table => table.TableName.StartsWith("Storages")),
                DependOnTables = new Dictionary<string, Models.DbTables.DbTableModel>
                {
                    { nameof(StorageModel.PharmacyId),  DbCommands.DbTables.FirstOrDefault(table => table.TableName.StartsWith("Pharmacies"))},
                },
                SubMenu = new[] {
                    new MenuItemModel {MenuItemName = strings.Create, MenuItemAction = InsertCommands.CreateDependentEntity<StorageModel, PharmacyModel>},
                    new MenuItemModel {MenuItemName = strings.Remove, MenuItemAction = DeleteCommands.RemoveEntity<StorageModel>},
                    new MenuItemModel {MenuItemName = strings.Back, MenuItemAction = Back},
                }
            },
            new MenuItemModel
            {
                MenuItemName = strings.BatchGoods,
                MenuItemAction = DisplayOperationMenu,
                DbTable = DbCommands.DbTables.FirstOrDefault(table => table.TableName.StartsWith("BatchGoods")),
                DependOnTables = new Dictionary<string, Models.DbTables.DbTableModel>
                {
                    { nameof(BatchGoodsModel.GoodsId),  DbCommands.DbTables.FirstOrDefault(table => table.TableName.StartsWith("Goods")) },
                    { nameof(BatchGoodsModel.StorageId),  DbCommands.DbTables.FirstOrDefault(table => table.TableName.StartsWith("Storages")) },
                },
                SubMenu = new[] {
                    new MenuItemModel {MenuItemName = strings.Create, MenuItemAction = InsertCommands.CreateDependentEntity<BatchGoodsModel, GoodsModel, StorageModel>},
                    new MenuItemModel {MenuItemName = strings.Remove, MenuItemAction = DeleteCommands.RemoveEntity<BatchGoodsModel>},
                    new MenuItemModel {MenuItemName = strings.Back, MenuItemAction = Back},
                }
            },
            new MenuItemModel
            {
                MenuItemName = strings.Exit,
                MenuItemAction = Exit,
            },
        };



        /// <summary>
        /// Для рпаботы с БД
        /// </summary>
        public static DbCommands DbCommands
        {
            get
            {
                if (_dbCommands == null)
                    _dbCommands = new DbCommands();
                return _dbCommands;
            }
        }


        /// <summary>
        /// Выбранный пункт главного меню
        /// </summary>
        internal static MenuItemModel SelectedMainMenuItem => _selectedMainMenuItem; 


        //#######################################################################################################################
        #region Menu actions

        /// <summary>
        /// Показать меню с задачами
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private static void DisplayOperationMenu()
        {
            _selectedMainMenuItem = _selectedMenuItem;
            DisplayMenu(_selectedMenuItem?.MenuItemName, _selectedMenuItem?.SubMenu);
            Back();
        }


        /// <summary>
        /// Выход из текущего меню
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private static void Exit()
        {
            DisplayToConsole.WaitForContinue(strings.WorkCompleted);
        }


        /// <summary>
        /// Вернуться в главное меню
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private static void Back()
        {
            DisplayMenu();
        }

        #endregion // Menu actions


        //#######################################################################################################################
        #region Menu navigation

        /// <summary>
        /// Показать меню
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


        /// <summary>
        /// Выбрать пункт меню
        /// </summary>
        /// <param name="menuTitle"></param>
        /// <param name="menu"></param>
        /// <returns></returns>
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

        #endregion // Menu navigation
    }
}
