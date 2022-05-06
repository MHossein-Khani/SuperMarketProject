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
           IWantTo = "دسته بندی را تعریف کنم",
           InOrderTo = "کالا را تعریف کنم"
           )]
    public class AddCategoryByTheNameThatExists : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly CategoryService _sut;
        private readonly UnitOfWork _unitOfWork;
        private readonly CategoryRepository _categoryRepository;
        private AddCategoryDto _addDto;
        Action expected;

        public AddCategoryByTheNameThatExists(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _categoryRepository = new EFCategoryRepository(_dataContext);
            _sut = new CategoryAppService(_categoryRepository, _unitOfWork);
        }

        [Given("دسته بندی با عنوان 'لبنیات' در فهرست دسته بندی وجود دارد")]
        public void Given()
        {
            var category = new Category
            {
                Name = "لبنیات"
            };
            _dataContext.Manipulate(_ => _.Categories.Add(category));
        }

        [When("دسته بندی با عنوان 'لبنیات' را تعریف میکنیم")]
        public void When()
        {
             _addDto = new AddCategoryDto
            {
                Name = "لبنیات"
            };

            expected =() =>  _sut.Add(_addDto);
        }

        [Then("تنها یک دسته بندی با عنوان ' لبنیات' باید در فهرست دسته بندی کالا وجود داشته باشد")]
        public void Then()
        {
            _dataContext.Categories.Where(p => p.Name == _addDto.Name)
                .Should().HaveCount(1);
        }

        [And("خطایی با عنوان 'عنوان دسته بندی کالا تکراریست ' باید رخ دهد")]
        public void And()
        {
            expected.Should().ThrowExactly<CategoryNameIsAlreadyExistException>();
        }

        [Fact]
        public void Run()
        {
            Given();
            When();
            Then();
            And();
        }
    }
}
