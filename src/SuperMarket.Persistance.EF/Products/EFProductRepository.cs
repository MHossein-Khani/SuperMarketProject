using SuperMarket.Services.Products.Cantracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Persistance.EF.Products
{
    public class EFProductRepository : ProductRepository
    {
        private readonly EFDataContext _dataContext;

        public EFProductRepository(EFDataContext dataContext)
        {
            _dataContext = dataContext;
        }
    }
}
