using PharmCompany.ConsoleApp.Models;
using PharmCompany.ConsoleApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PharmCompany.ConsoleApp.Menu.MenuCommands
{
    /// <summary>
    /// Получение данных из БД
    /// </summary>
    internal static class SelectCommands
    {

        /// <summary>
        ///  Получить Список сущностей
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        internal static IEnumerable<T> GetAll<T>(string tableName = "")
            where T : new()
        {
            if (string.IsNullOrEmpty(tableName))
                tableName = MenuManager.SelectedMainMenuItem.DbTable.TableName;

            var sqlCommand = $"SELECT * FROM [{tableName}]";
            var dataRecords = MenuManager.DbCommands.SelectCommand(sqlCommand).Result;

            if (dataRecords == null
                || dataRecords.Length < 1
                )
            {
                DisplayToConsole.WaitForContinue("Нет данных");
                return null;
            }

            return CommonCommands.GetModels<T>(dataRecords, MenuManager.SelectedMainMenuItem);
        }


        /// <summary>
        /// Список товаров в Аптеке
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        internal static void GetGoods()
        {
            var table = MenuManager.DbCommands.DbTables.FirstOrDefault(t => t.TableName.StartsWith("Pharmacies"));
            var selectedGuid = CommonCommands.GetDependOnGuid<PharmacyModel>(table.TableName);
            if (string.IsNullOrEmpty(selectedGuid))
                return;

            var sqlCommand = $"SELECT t1.Name, SUM(t2.GoodsCount) as sum FROM Goods as t1 " +
                             $"JOIN BatchGoods as t2 ON t1.Id = t2.GoodsId " +
                             $"JOIN Storages as t3 ON t2.StorageId = t3.Id " +
                             $"where t3.PharmacyId = N'{selectedGuid}' " +
                             $"group by t1.Name";

            CommonCommands.SelectFromDB(sqlCommand);
        }

    }
}
