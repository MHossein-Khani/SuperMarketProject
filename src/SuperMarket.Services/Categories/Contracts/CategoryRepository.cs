using SuperMarket.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Services.Categories.Contracts
{
    public interface CategoryRepository
    {
        void Add(Category category);
        bool IsCategoryNameExist(string name);
        Category FindById(int id);
        List<GetCategoryDto> GetAll();
        void Delete(int id);
    }
}
