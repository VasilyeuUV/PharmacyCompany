using PharmCompany.ConsoleApp.Menu;
using System.Data;
using System.Data.SqlClient;


namespace PharmCompany.ConsoleApp
{
    internal class Program
    {
        private string _connectionString =
            $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=E:\\_projects\\.Net\\SpargoTech\\PharmacyCompany\\PharmacyCompany.DataBase\\PharmCompanyDb.mdf;Integrated Security=True";

        private SQLConnection _sqlConnection = null ;




        static void Main(string[] args)
        {
            MenuManager.DisplayMenu();
        }
    }
}