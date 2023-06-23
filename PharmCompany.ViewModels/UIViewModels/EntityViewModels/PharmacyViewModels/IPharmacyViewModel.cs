using PharmCompany.Entities.Operations.CombinedOperations;
using PharmCompany.Models.ObjectModels;

namespace PharmCompany.ViewModels.UIViewModels.EntityViewModels.PharmacyViewModels
{
    /// <summary>
    /// Контракт для вьюмодели аптек
    /// </summary>
    public interface IPharmacyViewModel : ICreateRemoveable<PharmacyModel>
    {
    }
}