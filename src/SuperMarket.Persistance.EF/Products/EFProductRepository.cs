using SuperMarket.Entities;
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

        public void Add(Product product)
        {
            _dataContext.products.Add(product);
        }

        public bool IsProductCodeExist(string code)
        {
            return _dataContext.products.Any(p => p.Code == code);
        }

        public Product FindById(int id)
        {
            return _dataContext.products.Find(id);
        }

        public int countOfProductCode(string code)
        {
            return _dataContext.products.Where(p => p.Code == code).Count();    
        }

        public List<GetProductDto> Get(int categoryId)
        {
            return _dataContext.products.
                Where(p => p.CategoryId == categoryId).
                Select(p => new GetProductDto
                {
                    Code = p.Code,
                    Name = p.Name,
                    Inventory = p.Inventory,
                    Price = p.Price,
                    MinimumInventory = p.MinimumInventory,
                    CategoryId = p.CategoryId
                }).ToList();
                

        }
    }
}
