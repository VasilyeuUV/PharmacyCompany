using PharmCompany.ConsoleApp.DbLogics;
using PharmCompany.ConsoleApp.Models;
using PharmCompany.ConsoleApp.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace PharmCompany.ConsoleApp.Menu.MenuCommands
{
    /// <summary>
    /// Общие методы
    /// </summary>
    internal static class CommonCommands
    {


        /// <summary>
        /// Создание модели и получение его свойств
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>Свойства модели</returns>
        internal static T CreateModel<T>(IDataRecord dataRecord = null)
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


        /// <summary>
        /// Получить список моделей из базы данных
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataRecords"></param>
        /// <param name="selectedMainMenuItem"></param>
        /// <returns></returns>
        internal static IEnumerable<T> GetModels<T>(IDataRecord[] dataRecords, MenuItemModel selectedMainMenuItem)
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
        /// Получение значений свойство объекта класса
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        internal static PropertyInfo[] GetProperties<T>(T model)
            where T : new()
        {
            return model == null
                ? null
                : typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }



        /// <summary>
        /// Получить параметры основного объекта
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        internal static Dictionary<string, string> GetMainObjectProperties<T>()
            where T : new()
        {
            T model = CreateModel<T>();
            var propertiesDict = GetProperties(model)
                ?.Where(p => MenuManager.SelectedMainMenuItem.DbTable.ColumnNames.Contains(p.Name))
                .ToDictionary(p => p.Name, p => $"N'{p.GetValue(model)}'");

            return propertiesDict == null
                || propertiesDict.Count() < 1
                || !CheckProperties(MenuManager.SelectedMainMenuItem.DbTable.ColumnNames, propertiesDict.Keys.ToList())
                ? null
                : propertiesDict;
        }


        /// <summary>
        /// Получить родительский guid
        /// </summary>
        /// <param name="dependDict"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal static string GetDependOnGuid<T>(string tableName = "")
            where T : new()
        {
            var masterModels = SelectCommands.GetAll<T>(tableName);
            DisplayToConsole.DisplayBody(
                masterModels.Select(g => (g as ANameableEntityBase).DisplayFormat).ToArray(),
                isNumberedList: true
                );
            var guids = masterModels
                .Select(g => (g as ANameableEntityBase).Id.ToString())
                .ToArray();

            var selectedMasterNumber = InputNumber(guids.Length);
            if (selectedMasterNumber == null 
                || selectedMasterNumber < 1)
                return null;

            var selectedGuid = guids[selectedMasterNumber.Value - 1];
            return selectedGuid;
        }


        /// <summary>
        /// Проверка соответствия имён параметрав наименованиям колонок таблиц
        /// </summary>
        /// <param name="dbTable"></param>
        /// <param name="dictKeys"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal static bool CheckProperties(IEnumerable<string> columnNames, IEnumerable<string> dictKeys)
        {
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
        /// Добавить в БД
        /// </summary>
        /// <param name="propertiesDict"></param>
        internal static void InsertToDB(Dictionary<string, string> propertiesDict)
        {
            string columns = string.Join(", ", propertiesDict.Keys.ToArray());
            string values = string.Join(", ", propertiesDict.Values.ToArray());

            var sqlCommand = $"INSERT INTO [{MenuManager.SelectedMainMenuItem.DbTable.TableName}] ({columns}) VALUES ({values})";
            var result = MenuManager.DbCommands.ExecuteCommand(sqlCommand);

            if (result > 0)
                DisplayToConsole.WaitForContinue(strings.ObjectAdded);
            else
                DisplayToConsole.WaitForContinue(strings.ObjectNotAdded);
        }


        /// <summary>
        /// Получение и отображение данных в консоли
        /// </summary>
        /// <param name="sqlCommand"></param>
        internal static void SelectFromDB(string sqlCommand)
        {
            var dataRecords = MenuManager.DbCommands.SelectCommand(sqlCommand).Result;

            if (dataRecords == null
                || dataRecords.Length < 1
                )
            {
                DisplayToConsole.WaitForContinue(strings.NoData);
                return;
            }

            List<string[]> resultLst = new List<string[]>();
            for (int i = 0; i < dataRecords.Length; i++)
            {
                var dataRecord = dataRecords[i];
                Dictionary<string, string> dbValues = new Dictionary<string, string>();
                for (int j = 0; j < dataRecord.FieldCount; j++)
                {
                    dbValues.Add(dataRecord.GetName(j), dataRecord.GetValue(j).ToString());
                }
                var lst = dbValues.Values.ToArray();
                resultLst.Add(lst);
            }

            DisplayToConsole.DisplayBody(
                resultLst.Select(v => $"{v[0]} ({v[1]})").ToArray(),
                isNumberedList: true
                );
            DisplayToConsole.WaitForContinue();
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
