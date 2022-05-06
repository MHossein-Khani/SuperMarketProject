using FluentAssertions;
using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using SuperMarket.Infrastructure.Test;
using SuperMarket.Persistance.EF;
using SuperMarket.Persistance.EF.Categories;
using SuperMarket.Services.Categories;
using SuperMarket.Services.Categories.Contracts;
using SuperMarket.Services.Categories.Exceptions;
using SuperMarket.Specs.Infrastructure;
using System;
using System.Linq;
using Xunit;
using static SuperMarket.Specs.BDDHelper;

namespace SuperMarket.Specs.Categories
{
    [Scenario("تعریف دسته بندی")]
    [Feature("",
           AsA = "فروشنده ",
           IWantTo = "دسته بندی را ویرایش کنم",
           InOrderTo = "تا بتوانم در دسته بندی ها تغییر ایجاد کنم"
           )]
    public class UpdateCategory : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly CategoryService _sut;
        private readonly UnitOfWork _unitOfWork;
        private readonly CategoryRepository _categoryRepository;
        private Category _category;
        private UpdateCategoryDto _dto;

        public UpdateCategory(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _categoryRepository = new EFCategoryRepository(_dataContext);
            _sut = new CategoryAppService(_categoryRepository, _unitOfWork);
        }

        [Given("دسته بندی با عنوان 'لبنیات' در فهرست دسته بندی وجود دارد")]
        public void Given()
        {
            _category = new Category
            {
                Name = "لبنیات"
            };
            _dataContext.Manipulate(_ => _.Categories.Add(_category));
        }

        [When("دسته بندی 'لبنیات' را به 'خشکبار' تغییر میدهیم")]
        public void When()
        {
             _dto = new UpdateCategoryDto
            {
                Name = "خشکبار"
            };

            _sut.Update(_dto, _category.Id);
        }

        [Then("دسته 'لبنیات' باید به 'خشکبار' تغییر پیدا کرده باشد")]
        public void Then()
        {
            var expected = _dataContext.Categories.FirstOrDefault(p => p.Id == _category.Id);
            expected.Name.Should().Be(_dto.Name);
        }

        [Fact]
        public void Run()
        {
            Given();
            When();
            Then();
        }
    }
}
