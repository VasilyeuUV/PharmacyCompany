using System.Windows.Input;

namespace PharmCompany.ViewModels.Commands
{
    public class AsyncCommand : ICommand
    {
        private readonly Func<Task> _execute;


        /// <summary>
        /// CTOR
        /// </summary>
        public AsyncCommand(Func<Task> execute)
        {
            this._execute = execute;
        }

        //################################################################################################
        #region ICommand

        public event EventHandler? CanExecuteChanged;


        public bool CanExecute(object? parameter)
        {
            return true;                // - безусловное выполнение команды
        }


        public async void Execute(object? parameter)
        {
            await _execute.Invoke();
        }

        #endregion // ICommand

    }
}
