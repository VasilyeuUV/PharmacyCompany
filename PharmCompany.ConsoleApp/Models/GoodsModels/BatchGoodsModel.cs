using System;
namespace PharmCompany.ConsoleApp.Models
{
    public class BatchGoodsModel : ANameableEntityBase
    {
        public GoodsModel Goods { get; set; }
    }
}
