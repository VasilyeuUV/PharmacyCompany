using System.Collections.Generic;

namespace PharmCompany.ConsoleApp.Models.DbTables
{
    /// <summary>
    /// Модель таблицы базы данных
    /// </summary>
    internal class DbTableModel
    {
        /// <summary>
        /// Наименование таблицы
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// Список имён колонок таблицы
        /// </summary>
        public IEnumerable<string> ColumnNames { get; set; }
    }
}
