using FluentAssertions;
using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using SuperMarket.Infrastructure.Test;
using SuperMarket.Persistance.EF;
using SuperMarket.Persistance.EF.Categories;
using SuperMarket.Services.Categories;
using SuperMarket.Services.Categories.Contracts;
using SuperMarket.Services.Categories.Exceptions;
using System;
using Xunit;

namespace SuperMarket.Services.Test.Unit.Categories
{
    public class CategoryServiceTests
    {
        private readonly EFDataContext _dataContext;
        private readonly UnitOfWork _unitOfWork;
        private readonly CategoryRepository _categoryRepository;
        private readonly CategoryService _sut;

        public CategoryServiceTests()
        {
            _dataContext = new EFInMemoryDataBase()
                .CreateDataContext<EFDataContext>();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _categoryRepository = new EFCategoryRepository(_dataContext);
            _sut = new CategoryAppService(_categoryRepository, _unitOfWork);
        }

        [Fact]
        public void Add_adds_category_properly()
        {
            var dto = new AddCategoryDto
            {
                Name = "لبنیات"
            };

            _sut.Add(dto);
            
            _dataContext.Categories.Should().Contain(p => p.Name == dto.Name);
        }

        [Fact]
        public void Throw_Exception_if_CategoryNameIsAlreadyExistException_when_add_duplicate_name_in_category()
        {
            var category = new Category
            {
                Name = "لبنیات"
            };
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var dto = new AddCategoryDto
            {
                Name = category.Name,
            };

            Action expected = () => _sut.Add(dto);
            expected.Should().ThrowExactly<CategoryNameIsAlreadyExistException>();
        }


    }
}
