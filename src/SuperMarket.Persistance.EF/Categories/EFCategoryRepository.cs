using SuperMarket.Entities;
using SuperMarket.Services.Categories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Persistance.EF.Categories
{
    public class EFCategoryRepository : CategoryRepository
    {
        private readonly EFDataContext _dataContext;

        public EFCategoryRepository(EFDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void Add(Category category)
        {
            _dataContext.Add(category);
        }

        public void Delete(Category category)
        {
            _dataContext.Categories.Remove(category);
        }

        public Category FindById(int id)
        {
            return _dataContext.Categories.Find(id);
        }

        public List<GetCategoryDto> GetAll()
        {
            return _dataContext.Categories.
                Select(p => new GetCategoryDto
                {
                    Id = p.Id,
                    Name = p.Name,
                }).ToList();
        }

        public bool IsCategoryHasProduct(int categoryId)
        {
            return _dataContext.products.Any(p => p.CategoryId == categoryId);
        }

        public bool IsCategoryNameExist(string name)
        {
           return _dataContext.Categories.Any(p => p.Name == name);
        }
    }
}
