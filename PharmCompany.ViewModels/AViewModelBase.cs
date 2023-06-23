using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PharmCompany.ViewModels
{
    /// <summary>
    /// Базовая вьюмодель
    /// </summary>
    public abstract class AViewModelBase : INotifyPropertyChanged
    {

        /// <summary>
        /// Сеттер. Задача - обновлять значение свойства, для которого определено поле
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field">ссылка на поле свойства</param>
        /// <param name="value">новое значение</param>
        /// <param name="propertyName">имя свойства</param>
        /// <returns></returns>
        protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (Equals(field, value))
                return false;

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }


        /// <summary>
        /// Обработчик события
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));



        //######################################################################################################
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion // INotifyPropertyChanged
    }
}
