﻿using PharmCompany.ConsoleApp.Menu;
using PharmCompany.ConsoleApp.Models.DbTables;
using PharmCompany.ConsoleApp.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace PharmCompany.ConsoleApp.DbLogics
{
    /// <summary>
    /// Выполнение команд БД
    /// </summary>
    internal class DbCommands
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["PharmCompanyDB"].ConnectionString;
        private IEnumerable<DbTableModel> _dbTables;


        /// <summary>
        /// Список таблиц базы данных
        /// </summary>
        public IEnumerable<DbTableModel> DbTables
        {
            get
            {
                if (_dbTables == null)
                    _dbTables = GetDbTableNames();
                return _dbTables;
            }
        }


        /// <summary>
        /// Выполнение операции с БД
        /// </summary>
        /// <param name="sqlText"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal int? ExecuteCommand(string sqlCommand)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(sqlCommand, connection))
                {
                    int? number = null;
                    try
                    {
                        connection.Open();
                        number = command.ExecuteNonQuery();
                    }
                    catch (SqlException e)
                    {
                        DisplayToConsole.WaitForContinue(e.Message);
                    }
                    catch (Exception e)
                    {
                        DisplayToConsole.WaitForContinue(e.Message);
                    }
                    return number;
                }
            }
        }



        /// <summary>
        /// Получение данных с базды данных
        /// </summary>
        /// <param name="sqlCommand"></param>
        /// <param name="selectedMainMenuItem"></param>
        /// <returns></returns>
        internal async Task<IDataRecord[]> SelectCommand(string sqlCommand)
        {
            //Dictionary<string, string> dbValues = new Dictionary<string, string>();


            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(sqlCommand, connection))
                {
                    await connection.OpenAsync();
                    try
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                                return reader.Cast<IDataRecord>().ToArray();


                            //{

                            //    //for (int i = 0; i < properties.Length; i++)
                            //    //{
                            //    //    var prop = properties[i];

                            //    //    for (int j = 0; j < prop.FieldCount; j++)
                            //    //    {
                            //    //        dbValues.Add(properties[i].GetName(j), properties[i].GetValue(j).ToString());

                            //    //    }

                            //    //}


                            //    //var propertiesDict = properties?.ToDictionary(p => p.GetName(), p => $"N'{p.GetValue(model)}'");



                            //    //List<string> list = (from IDataRecord r in reader select (string)r["FieldName"])
                            //    //                     .ToDictionary();

                            //    //while (reader.Read())
                            //    //{

                            //    //    var id = reader[];
                            //    //    var name = reader.GetValue(1);
                            //    //    var age = reader.GetValue(2);

                            //    //    Console.WriteLine($"{id} \t {name} \t {age}");
                            //    //}
                            //}
                        }
                    }              
                    catch (SqlException e)
                    {
                        DisplayToConsole.WaitForContinue(e.Message);
                    }
                    catch (Exception e)
                    {
                        DisplayToConsole.WaitForContinue(e.Message);
                    }
                    return null;
                }
            }
        }






        /// <summary>
        /// Получить имена таблиц БД
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private IEnumerable<DbTableModel> GetDbTableNames()
        {
            List<DbTableModel> tables = new List<DbTableModel>();

            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    DataTable schemaTables = connection.GetSchema("Tables");
                    foreach (DataRow row in schemaTables.Rows)
                    {
                        DbTableModel table = new DbTableModel();
                        table.TableName = row["TABLE_NAME"].ToString();

                        string[] columnRestrictions = new String[4];
                        columnRestrictions[2] = table.TableName;
                        DataTable allColumnsSchemaTable = connection.GetSchema("Columns", columnRestrictions);

                        List<string> columnNames = new List<string>();
                        foreach (DataRow row1 in allColumnsSchemaTable.Rows)
                        {
                            var columnName = row1.Field<string>("COLUMN_NAME");
                            columnNames.Add(columnName);
                        }
                        table.ColumnNames = columnNames;
                        tables.Add(table);
                    }
                }
                catch (SqlException e)
                {
                    DisplayToConsole.WaitForContinue(e.Message);
                }
                catch (Exception e)
                {
                    DisplayToConsole.WaitForContinue(e.Message);
                }
                return tables;
            }
        }

    }
}
