using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using SuperMarket.Services.Categories.Contracts;

namespace SuperMarket.Services.Categories
{
    public class CategoryAppService : CategoryService
    {
        private readonly CategoryRepository _categoryRepository;
        private readonly UnitOfWork _unitOfWork;
        public CategoryAppService(CategoryRepository categoryRepository, UnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public void Add(AddCategoryDto dto)
        {
            var category = new Category
            {
                Name = dto.Name,
            };
            _categoryRepository.Add(category);
            _unitOfWork.Commit();
        }
    }
}
