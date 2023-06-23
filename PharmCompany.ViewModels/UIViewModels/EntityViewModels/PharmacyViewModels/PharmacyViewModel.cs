using PharmCompany.Models.ObjectModels;

namespace PharmCompany.ViewModels.UIViewModels.EntityViewModels.PharmacyViewModels
{
    /// <summary>
    /// Вьюмодель для аптек
    /// </summary>
    public class PharmacyViewModel : AViewModelBase, IPharmacyViewModel
    {
        private PharmacyModel? _selectedPharmacy;


        /// <summary>
        /// Выбранная аптека
        /// </summary>
        public PharmacyModel? SelectedPharmacy 
        { 
            get => _selectedPharmacy; 
            private set => Set(ref _selectedPharmacy, value); 
        }


        //############################################################################################
        #region IPharmacyViewModel

        public async Task<PharmacyModel> AddAsync(PharmacyModel item)
        {
            throw new NotImplementedException();
        }

        public bool Remove(Guid id)
        {
            throw new NotImplementedException();
        }

        #endregion // IPharmacyViewModel

    }
}
