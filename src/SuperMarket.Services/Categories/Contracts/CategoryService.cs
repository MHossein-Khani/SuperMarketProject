using System.Collections.Generic;

namespace SuperMarket.Services.Categories.Contracts
{
    public interface CategoryService
    {
        void Add(AddCategoryDto dto);
        void Update(UpdateCategoryDto dto, int id);
        List<GetCategoryDto> GetAll();
        void Delete(int id);
    }
}
