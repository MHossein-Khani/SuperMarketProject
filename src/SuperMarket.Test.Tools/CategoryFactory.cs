using SuperMarket.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Test.Tools
{
    public static class CategoryFactory
    {
        public static Category CreateCategory(string name)
        {
            return new Category()
            {
                Name = name
            };
        }
    }
}
