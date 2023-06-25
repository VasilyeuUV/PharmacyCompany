using PharmCompany.ConsoleApp.Models;
using System.Collections.Generic;

namespace PharmCompany.ConsoleApp.ViewModels
{
    internal class MainViewModel
    {

        public IEnumerable<GoodsModel> Goods { get; set; }
        public IEnumerable<PharmacyModel> Pharmacies { get; set; }
    }
}
