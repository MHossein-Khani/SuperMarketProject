using SuperMarket.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Test.Tools
{
    public static class ProductFactory
    {
        public static Product CreatProduct(int ctegoryId)
        {
            return new Product
            {
                Code = "1",
                Name = "شیر کاله",
                MinimumInventory = 5,
                Price = 5000,
                Inventory = 10,
                CategoryId = ctegoryId
            };
        }
    }
}
