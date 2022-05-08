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
using SuperMarket.Test.Tools;
using System;
using Xunit;
using static SuperMarket.Specs.BDDHelper;

namespace SuperMarket.Specs.Categories
{
    [Scenario("حذف دسته بندی")]
    [Feature("",
           AsA = "فروشنده ",
           IWantTo = "دسته بندی را حذف کنم",
           InOrderTo = " نتوانم کالا در آن تعریف کنم"
           )]
    public class DeleteCategoryWhenProductIsDefinedInIt : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly CategoryService _sut;
        private readonly UnitOfWork _unitOfWork;
        private readonly CategoryRepository _categoryRepository;
        private Category _category;
        Action expected;

        public DeleteCategoryWhenProductIsDefinedInIt(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _categoryRepository = new EFCategoryRepository(_dataContext);
            _sut = new CategoryAppService(_categoryRepository, _unitOfWork);
        }

        [Given(" دسته بندی با عنوان 'لبنیات' در فهرست دسته بندی وجود دارد")]
        public void Given()
        {
            _category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(_category));
        }

        [And("در دسته بندی 'لبنیات' کالای 'شیر' وجود دارد")]
        public void And()
        {
            var product = ProductFactory.CreatProduct("1", 10, _category.Id);
            _dataContext.Manipulate(_ => _.products.Add(product));
        }

        [When("دسته بندی 'لبنیات' را حذف میکنیم")]
        public void When()
        {
            expected = () => _sut.Delete(_category.Id);
        }

        [Then("دسته بندی با عنوان ' لبنیات' باید در فهرست دسته بندی کالا وجود داشته باشد")]
        public void Then()
        {
            _dataContext.Categories.Should().HaveCount(1);
        }

        [And("خطایی با عنوان 'در این دسته بندی کالا تعریف شده است' باید رخ دهد")]
        public void ThenAnd()
        {
            expected.Should().ThrowExactly<InThisCategoryProductIsDefinedException>();
        }

        [Fact]
        public void Run()
        {
            Given();
            And();
            When();
            Then();
            ThenAnd();
        }
    }
}
