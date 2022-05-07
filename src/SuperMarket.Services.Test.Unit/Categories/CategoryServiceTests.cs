using FluentAssertions;
using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using SuperMarket.Infrastructure.Test;
using SuperMarket.Persistance.EF;
using SuperMarket.Persistance.EF.Categories;
using SuperMarket.Services.Categories;
using SuperMarket.Services.Categories.Contracts;
using SuperMarket.Services.Categories.Exceptions;
using SuperMarket.Test.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public void Throw_exception_if_CategoryNameIsAlreadyExistException_when_add_duplicate_name_in_category()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var dto = new AddCategoryDto
            {
                Name = category.Name,
            };

            Action expected = () => _sut.Add(dto);
            expected.Should().ThrowExactly<CategoryNameIsAlreadyExistException>();
        }

        [Fact]
        public void Update_updates_category_properly()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var dto = new UpdateCategoryDto
            {
                Name = "خشکبار"
            };

            _sut.Update(dto, category.Id);

            var expected = _dataContext.Categories.FirstOrDefault(p => p.Id == category.Id);
            expected.Name.Should().Be(dto.Name);
        }

        [Fact]
        public void Throw_exception_if_CategoryNameIsAlreadyExistException_when_update_a_category_name()
        {
            var category1 = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category1));

            var category2 = CategoryFactory.CreateCategory("خشکبار");
            _dataContext.Manipulate(_ => _.Categories.Add(category2));

            var dto = new UpdateCategoryDto
            {
                Name = "لبنیات"
            };

            Action expected = () => _sut.Update(dto, category2.Id);
            expected.Should().ThrowExactly<CategoryNameIsAlreadyExistException>();
        }

        [Fact]
        public void Throw_exception_if_CategoryDoesNotExistException_when_update_a_category_that_does_not_exist_in_database()
        {
            var dto = new UpdateCategoryDto
            {
                Name = "test"
            };
            var fakeId = 100;

            Action expected = () => _sut.Update(dto, fakeId);
            expected.Should().ThrowExactly<CategoryDoesNotExistException>();
        }

        [Fact]
        public void GetAll_returns_all_category_properly()
        {
            CreateCategoriesInDataBase();

            var expected = _sut.GetAll();

            expected.Should().HaveCount(2);
            expected.Should().Contain(p => p.Name == "لبنیات");
            expected.Should().Contain(p => p.Name == "خشکبار");
        }

        [Fact]
        public void Delete_deletes_a_category_properly()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            _sut.Delete(category.Id);

            _dataContext.Categories.Should().HaveCount(0);
        }

        private void CreateCategoriesInDataBase()
        {
            var categories = new List<Category>
            {
                new Category { Name = "لبنیات"},
                new Category { Name = "خشکبار"},
            };
            _dataContext.Manipulate(_ =>
            _.Categories.AddRange(categories));
        }
    }
}
