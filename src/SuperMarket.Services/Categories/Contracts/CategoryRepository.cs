using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Services.Categories.Contracts
{
    public interface CategoryRepository : Repository
    {
        void Add(Category category);
        bool IsCategoryNameExist(string name);
        Category FindById(int id);
        List<GetCategoryDto> GetAll();
        void Delete(Category category);
        bool IsCategoryHasProduct(int categoryId);
    }
}
