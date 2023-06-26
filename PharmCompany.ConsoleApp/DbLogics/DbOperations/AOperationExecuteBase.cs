using System.Configuration;

namespace PharmCompany.ConsoleApp.DbLogics.DbOperations
{
    internal abstract class AOperationExecuteBase
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["PharmCompanyDB"].ConnectionString;




    }
}
