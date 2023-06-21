using PharmCompany.Entities.BaseEntities;

namespace PharmCompany.Models.GoodsModels
{
    public class GoodsModel : ANameableEntityBase
    {

        //#################################################################################################################
        #region CTOR

        public GoodsModel(string name) : base(name) { }
        public GoodsModel(string name, Guid guid) : base(name, guid) { }

        #endregion // CTOR

    }
}
