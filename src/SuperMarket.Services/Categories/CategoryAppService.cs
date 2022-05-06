using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using SuperMarket.Services.Categories.Contracts;
using SuperMarket.Services.Categories.Exceptions;

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

            var isNameExist = _categoryRepository
                .IsCategoryNameExist(dto.Name);
            if (isNameExist)
            {
                throw new CategoryNameIsAlreadyExistException();
            }

            _categoryRepository.Add(category);
            _unitOfWork.Commit();
        }

        public void Update(UpdateCategoryDto dto, int id)
        {
            var category = _categoryRepository.FindById(id);

            category.Name = dto.Name;

            _unitOfWork.Commit();
        }
    }
}
