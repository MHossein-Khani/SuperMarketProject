using FluentAssertions;
using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using SuperMarket.Infrastructure.Test;
using SuperMarket.Persistance.EF;
using SuperMarket.Persistance.EF.Categories;
using SuperMarket.Services.Categories;
using SuperMarket.Services.Categories.Contracts;
using SuperMarket.Specs.Infrastructure;
using SuperMarket.Test.Tools;
using System.Linq;
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
    public class DeleteCategory : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly CategoryService _sut;
        private readonly UnitOfWork _unitOfWork;
        private readonly CategoryRepository _categoryRepository;
        private Category _category;

        public DeleteCategory(ConfigurationFixture configuration) : base(configuration)
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

        [And("هیچ کالایی در دسته بندی 'لبنیات' وجود ندارد")]
        public void And()
        {
        }

        [When("دسته بندی 'لبنیات' را حذف میکنیم")]
        public void When()
        {
            _sut.Delete(_category.Id);
        }

        [Then("دسته بندی 'لبنیات' در فهرست دسته بندی نباید وجود داشته باشد")]
        public void Then()
        {
            _dataContext.Categories.Should().HaveCount(0);
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
