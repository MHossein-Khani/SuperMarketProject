using SuperMarket.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Services.Products.Cantracts
{
    public interface ProductRepository
    {
        void Add(Product product);
    }
}
