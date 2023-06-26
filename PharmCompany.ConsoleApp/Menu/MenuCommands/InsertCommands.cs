using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PharmCompany.ConsoleApp.Menu.MenuCommands
{
    /// <summary>
    /// Команды меню для добавления сущностей
    /// </summary>
    internal static class InsertCommands
    {

        /// <summary>
        /// Создание и сохранение объекта
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <exception cref="InvalidOperationException"></exception>
        internal static void CreateEntity<T>()
            where T : new()
        {
            Dictionary<string, string> propertiesDict = null;

            T model = CommonCommands.CreateModel<T>();
            propertiesDict = CommonCommands.GetProperties(model)?.Where(p => p.Name != "DisplayFormat")
                .ToDictionary(p => p.Name, p => $"N'{p.GetValue(model)}'");

            if (propertiesDict == null
                || propertiesDict.Count() < 1
                || !CommonCommands.CheckProperties(MenuManager.SelectedMainMenuItem.DbTable.ColumnNames, propertiesDict.Keys.ToList())
                )
                throw new InvalidOperationException(strings.ErrorProperties);

            CommonCommands.InsertToDB(propertiesDict);
        }


        /// <summary>
        /// Создание и сохранение зависимого объекта
        /// </summary>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T1"></typeparam>
        internal static void CreateDependentEntity<T1, T2>()
            where T1 : new()
            where T2 : new()
        {
            var propertiesDict = CommonCommands.GetMainObjectProperties<T1>();
            if (propertiesDict == null)
                throw new InvalidOperationException(strings.ErrorProperties);

            var modelProperties = typeof(T1).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var dependOnTable = MenuManager.SelectedMainMenuItem.DependOnTables.First();

            var selectedGuid = CommonCommands.GetDependOnGuid<T2>(dependOnTable.Value.TableName);
            if (string.IsNullOrEmpty(selectedGuid))
                return;

            var dependOnTables = MenuManager.SelectedMainMenuItem.DependOnTables.ToArray();
            var id = propertiesDict.FirstOrDefault(k => k.Key == dependOnTable.Key).Key;
            if (!string.IsNullOrEmpty(id))
                propertiesDict[id] = $"N'{selectedGuid}'";

            CommonCommands.InsertToDB(propertiesDict);
        }


        /// <summary>
        /// Создание и сохранение объекта, зависимого от нескольких 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <exception cref="InvalidOperationException"></exception>
        internal static void CreateDependentEntity<T1, T2, T3>()
               where T1 : new()
               where T2 : new()
               where T3 : new()
        {
            var propertiesDict = CommonCommands.GetMainObjectProperties<T1>();
            if (propertiesDict == null)
                throw new InvalidOperationException(strings.ErrorProperties);

            var selectedGuid = string.Empty;
            var dependOnTables = MenuManager.SelectedMainMenuItem.DependOnTables.ToArray();

            for (int i = 0; i < dependOnTables.Length; i++)
            {
                if (i == 0)
                    selectedGuid = CommonCommands.GetDependOnGuid<T2>(dependOnTables[i].Value.TableName);
                if (i == 1)
                    selectedGuid = CommonCommands.GetDependOnGuid<T3>(dependOnTables[i].Value.TableName);

                if (string.IsNullOrEmpty(selectedGuid))
                    return;

                var id = propertiesDict.FirstOrDefault(k => k.Key == dependOnTables[i].Key).Key;
                if (!string.IsNullOrEmpty(id))
                    propertiesDict[id] = $"N'{selectedGuid}'";
            }
            CommonCommands.InsertToDB(propertiesDict);
        }
    }
}
