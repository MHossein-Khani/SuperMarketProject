using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Services.Products.Cantracts
{
    public interface ProductRepository : Repository
    {
        void Add(Product product);
        bool IsProductCodeExist(string code);
        Product FindById(int id);
        int countOfProductCode(string code);
        List<GetProductDto> Get(int categoryId);
        List<GetProductDto> GetAll();
        void Delete(Product product);
        bool IsProductUsedInSalesInvoice(int productId);
        int NumberOfProductInventory(int productId);
    }
}
