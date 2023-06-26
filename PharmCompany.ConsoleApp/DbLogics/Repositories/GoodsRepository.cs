using PharmCompany.ConsoleApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmCompany.ConsoleApp.DbLogics.Repositories
{
    public class GoodsRepository : IRepository<GoodsModel>
    {

        //############################################################################################
        #region IRepository<GoodsModel>

        public Task<GoodsModel> AddAsync(GoodsModel item)
        {
            throw new NotImplementedException();
        }


        public Task<IEnumerable<GoodsModel>> GetAllAsync()
        {
            throw new NotImplementedException();
        }


        public bool Remove(Guid id)
        {
            throw new NotImplementedException();
        }


        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion // IRepository<GoodsModel>

    }
}
