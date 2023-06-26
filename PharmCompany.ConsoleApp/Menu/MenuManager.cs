﻿using PharmCompany.ConsoleApp.DbLogics;
using PharmCompany.ConsoleApp.Models;
using PharmCompany.ConsoleApp.Models.DbTables;
using PharmCompany.ConsoleApp.Services;
using PharmCompany.ConsoleApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;

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
                    //new MenuItemModel {MenuItemName = "Показать список", MenuItemAction = GetAll},
                    new MenuItemModel {MenuItemName = strings.Create, MenuItemAction = CreateEntity<GoodsModel>},
                    new MenuItemModel {MenuItemName = strings.Remove, MenuItemAction = RemoveEntity<GoodsModel>},
                    new MenuItemModel {MenuItemName = strings.Back, MenuItemAction = Back},
                }
            },
            new MenuItemModel
            {
                MenuItemName = strings.Pharmacy,
                MenuItemAction = DisplayOperationMenu,
                DbTable = DbCommands.DbTables.FirstOrDefault(table => table.TableName.StartsWith("Pharmacies")),
                SubMenu = new[] {
                    //new MenuItemModel {MenuItemName = "Показать список", MenuItemAction = GetAll},
                    new MenuItemModel {MenuItemName = strings.Create, MenuItemAction = CreateEntity<PharmacyModel>},
                    new MenuItemModel {MenuItemName = strings.Remove, MenuItemAction = RemoveEntity<PharmacyModel>},
                    new MenuItemModel {MenuItemName = strings.Back, MenuItemAction = Back},
                }
            },
            new MenuItemModel
            {
                MenuItemName = strings.Storages,
                MenuItemAction = DisplayOperationMenu,
                DbTable = DbCommands.DbTables.FirstOrDefault(table => table.TableName.StartsWith("Storages")),
                //DbMasterTables = new [] {DbCommands.DbTables.FirstOrDefault(table => table.TableName.StartsWith("Pharmacies")) },
                SubMenu = new[] {
                    //new MenuItemModel {MenuItemName = "Показать список", MenuItemAction = GetAll},
                    new MenuItemModel {MenuItemName = strings.Create, MenuItemAction = CreateDependentEntity<StorageModel, PharmacyModel>},
                    new MenuItemModel {MenuItemName = strings.Remove, MenuItemAction = RemoveEntity<StorageModel>},
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
                    //new MenuItemModel {MenuItemName = "Показать список", MenuItemAction = GetAll},
                    new MenuItemModel {MenuItemName = strings.Create, MenuItemAction = CreateDependentEntity<BatchGoodsModel, GoodsModel, StorageModel>},
                    new MenuItemModel {MenuItemName = strings.Remove, MenuItemAction = RemoveEntity<BatchGoodsModel>},
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


        //#######################################################################################################################
        #region Menu actions

        //________________________________________________________________________________________________________________
        #region Main menu actions

        /// <summary>
        /// Показать меню с задачами
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private static void DisplayOperationMenu()
        {
            _selectedMainMenuItem = _selectedMenuItem;
            //DisplayMenu(_selectedMenuItem?.MenuItemName, _commonOperationMenu);
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

        #endregion // Main menu actions


        //________________________________________________________________________________________________________________
        #region Operation menu actions

        /// <summary>
        /// Создание объекта
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <exception cref="InvalidOperationException"></exception>
        private static void CreateEntity<T>()
            where T : new()
        {
            Dictionary<string, string> propertiesDict = null;

            T model = CreateModel<T>();
            propertiesDict = GetProperties(model)?.Where(p => p.Name != "DisplayFormat")
                .ToDictionary(p => p.Name, p => $"N'{p.GetValue(model)}'");

            if (propertiesDict == null
                || propertiesDict.Count() < 1
                || !CheckProperties(_selectedMainMenuItem.DbTable.ColumnNames, propertiesDict.Keys.ToList())
                )
                throw new InvalidOperationException(strings.ErrorProperties);

            InsertToDB(propertiesDict);
        }


        /// <summary>
        /// Создать зависимую сущность
        /// </summary>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T1"></typeparam>
        private static void CreateDependentEntity<T1, T2>()
            where T2 : new()
            where T1 : new()
        {
            Dictionary<string, string> propertiesDict = null;
            string[] guids = null;
            int? selectedMasterNumber = null;

            T1 dependentModel = CreateModel<T1>();
            propertiesDict = GetProperties(dependentModel)?.Where(p => p.Name != "DisplayFormat")
                .ToDictionary(p => p.Name, p => $"N'{p.GetValue(dependentModel)}'");

            if (propertiesDict == null
                || propertiesDict.Count() < 1
                || !CheckProperties(_selectedMainMenuItem.DbTable.ColumnNames, propertiesDict.Keys.ToList())
                )
                throw new InvalidOperationException(strings.ErrorProperties);

            var masterModels = GetAll<T2>(_selectedMainMenuItem.DependOnTables.FirstOrDefault().Value.TableName);
            DisplayToConsole.DisplayBody(
                masterModels.Select(g => (g as ANameableEntityBase).DisplayFormat).ToArray(),
                isNumberedList: true
                );
            guids = masterModels
                .Select(g => (g as ANameableEntityBase).Id.ToString())
                .ToArray();

            selectedMasterNumber = InputNumber(guids.Length);
            if (selectedMasterNumber == null)
                return;

            var selectedGuid = guids[selectedMasterNumber.Value - 1];

            var masterId = propertiesDict.FirstOrDefault(k => k.Key.EndsWith("Id") && k.Key.Length > 2).Key;
            if (!string.IsNullOrEmpty(masterId))
                propertiesDict[masterId] = $"N'{selectedGuid}'";

            InsertToDB(propertiesDict);
        }


        /// <summary>
        /// Создание зависимых объектов
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <exception cref="InvalidOperationException"></exception>
        private static void CreateDependentEntity<T1, T2, T3>()
               where T1 : new()
               where T2 : new()
               where T3 : new()
        {
            Dictionary<string, string> propertiesDict = null;
            string[] guids = null;
            int? selectedMasterNumber = null;

            T1 model = CreateModel<T1>();
            var modelProperties = typeof(T1).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            propertiesDict = GetProperties(model)
                ?.Where(p => _selectedMainMenuItem.DbTable.ColumnNames.Contains(p.Name))
                .ToDictionary(p => p.Name, p => $"N'{p.GetValue(model)}'");

            if (propertiesDict == null
                || propertiesDict.Count() < 1
                || !CheckProperties(_selectedMainMenuItem.DbTable.ColumnNames, propertiesDict.Keys.ToList())
                )
                throw new InvalidOperationException(strings.ErrorProperties);

            var selectedGuid = string.Empty;
            var dependOnTables = _selectedMainMenuItem.DependOnTables.ToArray();
            for (int i = 0; i < dependOnTables.Length; i++)
            {
                if (i == 0)
                    selectedGuid = GetDependOnGuid<T2>(modelProperties, dependOnTables[i]);
                if (i == 1)
                    selectedGuid = GetDependOnGuid<T3>(modelProperties, dependOnTables[i]);

                var masterId = propertiesDict.FirstOrDefault(k => k.Key == dependOnTables[i].Key).Key;
                if (!string.IsNullOrEmpty(masterId))
                    propertiesDict[masterId] = $"N'{selectedGuid}'";
            }
            InsertToDB(propertiesDict);
        }


        /// <summary>
        /// Получить родительский guid
        /// </summary>
        /// <param name="dependDict"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private static string GetDependOnGuid<T>(PropertyInfo[] modelProperties, KeyValuePair<string, DbTableModel> dependDict)
            where T: new()
        {
            var masterModels = GetAll<T>(dependDict.Value.TableName);
            DisplayToConsole.DisplayBody(
                masterModels.Select(g => (g as ANameableEntityBase).DisplayFormat).ToArray(),
                isNumberedList: true
                );
            var guids = masterModels
                .Select(g => (g as ANameableEntityBase).Id.ToString())
                .ToArray();

            var selectedMasterNumber = InputNumber(guids.Length);
            if (selectedMasterNumber == null)
                return null;

            var selectedGuid = guids[selectedMasterNumber.Value - 1];
            return selectedGuid;
        }



        private static void InsertToDB(Dictionary<string, string> propertiesDict)
        {
            string columns = string.Join(", ", propertiesDict.Keys.ToArray());
            string values = string.Join(", ", propertiesDict.Values.ToArray());

            var sqlCommand = $"INSERT INTO [{_selectedMainMenuItem.DbTable.TableName}] ({columns}) VALUES ({values})";
            var result = DbCommands.ExecuteCommand(sqlCommand);

            if (result > 0)
                DisplayToConsole.WaitForContinue(strings.ObjectAdded);
            else
                DisplayToConsole.WaitForContinue(strings.ObjectNotAdded);
        }




        /// <summary>
        /// Удаление объекта
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private static void RemoveEntity<T>()
            where T : class, new()
        {
            int? removeNumber = null;
            string[] guids = null;

            var models = GetAll<T>();
            if (models == null)
                return;

            DisplayToConsole.DisplayBody(models.Select(g => (g as ANameableEntityBase).DisplayFormat).ToArray(), isNumberedList: true);
            guids = models
                .Select(g => (g as ANameableEntityBase).Id.ToString())
                .ToArray();

            removeNumber = InputNumber(guids.Length);
            if (removeNumber == null)
                return;

            var sqlCommand = $"DELETE FROM [{_selectedMainMenuItem.DbTable.TableName}] WHERE [Id]='{guids[removeNumber.Value - 1]}'";
            var result = DbCommands.ExecuteCommand(sqlCommand);

            if (result > 0)
                DisplayToConsole.WaitForContinue(string.Format(strings.ObjectDeleted, removeNumber.Value));
            else
                DisplayToConsole.WaitForContinue(string.Format(strings.ObjectNotDeleted, removeNumber.Value));
        }

        #endregion // Operation menu actions


        /// <summary>
        ///  Получить Список сущностей
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private static IEnumerable<T> GetAll<T>(string tableName = "")
            where T : new()
        {
            if (string.IsNullOrEmpty(tableName))
                tableName = _selectedMainMenuItem.DbTable.TableName;

            var sqlCommand = $"SELECT * FROM [{tableName}]";
            var dataRecords = DbCommands.SelectCommand(sqlCommand).Result;

            if (dataRecords == null
                || dataRecords.Length < 1
                )
            {
                DisplayToConsole.WaitForContinue("Нет данных");
                return null;
            }

            return GetModels<T>(dataRecords, _selectedMainMenuItem);
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


        /// <summary>
        /// Получить список моделей из базы данных
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataRecords"></param>
        /// <param name="selectedMainMenuItem"></param>
        /// <returns></returns>
        private static IEnumerable<T> GetModels<T>(IDataRecord[] dataRecords, MenuItemModel selectedMainMenuItem)
            where T : new()
        {
            List<T> models = new List<T>();

            for (int i = 0; i < dataRecords.Length; i++)
            {
                T model = CreateModel<T>(dataRecords[i]);
                models.Add(model);
            }
            return models.ToArray();
        }


        /// <summary>
        /// Создание модели и получение его свойств
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>Свойства модели</returns>
        private static T CreateModel<T>(IDataRecord dataRecord = null)
            where T : new()
        {
            T model;

            if (dataRecord == null)
            {
                model = DisplayToConsole.CreateObject<T>();
                return model;
            }

            Dictionary<string, string> dbValues = new Dictionary<string, string>();
            for (int i = 0; i < dataRecord.FieldCount; i++)
            {
                dbValues.Add(dataRecord.GetName(i), dataRecord.GetValue(i).ToString());
            }

            model = new T();
            var propertyInfo = GetProperties(model)
                .ToList();

            foreach (var property in propertyInfo)
            {
                if (property.PropertyType == typeof(Guid))
                    property.SetValue(model, new Guid(dbValues[property.Name]));
                else
                    try
                    {
                        property.SetValue(model, dbValues[property.Name]);
                    }
                    catch (Exception)
                    {
                    }
            }
            return model;
        }


        //private static TDependent CreateDependentModel<TMaster, TDependent>(IDataRecord dataRecord = null)
        //    where TMaster : new()
        //    where TDependent : new()
        //{
        //    TDependent dependentModel;
        //    if (dataRecord == null)
        //    {
        //        dependentModel = DisplayToConsole.CreateDependentObject<TMaster, TDependent>();
        //        return dependentModel;
        //    }

        //    return new TDependent();
        //}




        /// <summary>
        /// Проверка соответствия имён параметрав наименованиям колонок таблиц
        /// </summary>
        /// <param name="dbTable"></param>
        /// <param name="dictKeys"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private static bool CheckProperties(IEnumerable<string> columnNames, IEnumerable<string> dictKeys)
        {
            //// - отличается количество параметров
            //if (columnNames.Count() != dictKeys.Count())
            //    return false;

            var names = columnNames.OrderBy(name => name).ToArray();
            var keys = dictKeys.OrderBy(key => key).ToArray();

            for (int i = 0; i < names.Length; i++)
            {
                if (names[i] != keys[i])
                    return false;
            }
            return true;
        }


        /// <summary>
        /// Получение значений свойство объекта класса
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        private static PropertyInfo[] GetProperties<T>(T model)
            where T : new()
        {
            return model == null
                ? null
                : typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }


        /// <summary>
        /// Ввод номера
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private static int? InputNumber(int valuesCount)
        {
            int? number = DisplayToConsole.InputIntValue(strings.EnterObjectNumber);
            if (number == null)
                return null;
            if (number.Value > valuesCount)
            {
                DisplayToConsole.WaitForContinue(string.Format(strings.ObjectNotExist, number.Value));
                return null;
            }
            return number;
        }
    }
}
