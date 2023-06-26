using PharmCompany.ConsoleApp.Services;

namespace PharmCompany.ConsoleApp.Menu.MenuCommands
{
    /// <summary>
    /// Команды удаления из БД
    /// </summary>
    internal static class DeleteCommands
    {
        /// <summary>
        /// Удаление объекта
        /// </summary>
        /// <typeparam name="T"></typeparam>
        internal static void RemoveEntity<T>()
            where T : class, new()
        {
            var selectedGuid = CommonCommands.GetDependOnGuid<T>();
            if (string.IsNullOrEmpty(selectedGuid))
                return;

            var sqlCommand = $"DELETE FROM [{MenuManager.SelectedMainMenuItem.DbTable.TableName}] WHERE [Id]='{selectedGuid}'";
            var result = MenuManager.DbCommands.ExecuteCommand(sqlCommand);

            if (result > 0)
                DisplayToConsole.WaitForContinue(strings.ObjectDeleted);
            else
                DisplayToConsole.WaitForContinue(strings.ObjectNotDeleted);
        }

    }
}
