using PharmCompany.Entities.BaseEntities;

namespace PharmCompany.Models.GoodsModels
{
    public class BatchGoodsModel : ANameableEntityBase
    {

        //#################################################################################################################
        #region CTOR

        public BatchGoodsModel(string name) : base(name) 
        {

        }

        public BatchGoodsModel(string name, Guid guid) : base(name, guid) { }

        #endregion // CTOR


        public GoodsModel Goods { get; set; }


    }
}
