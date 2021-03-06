using FluentAssertions;
using SuperMarket.Infrastructure.Application;
using SuperMarket.Persistance.EF;
using SuperMarket.Persistance.EF.Categories;
using SuperMarket.Services.Categories;
using SuperMarket.Services.Categories.Contracts;
using SuperMarket.Specs.Infrastructure;
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
    public class AddCategory : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly CategoryService _sut;
        private readonly UnitOfWork _unitOfWork;
        private readonly CategoryRepository _categoryRepository;
        private AddCategoryDto _addDto;

        public AddCategory(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _categoryRepository = new EFCategoryRepository(_dataContext);
            _sut = new CategoryAppService(_categoryRepository, _unitOfWork);
        }

        [Given("هیچ دسته بندی در فهرست دسته بندی وجود ندارد")]
        public void Given()
        {
            
        }

        [When("دسته بندی با عنوان 'لبنیات' را تعریف میکنیم")]
        public void When()
        {
             _addDto = new AddCategoryDto
            {
                Name = "لبنیات"
            };

            _sut.Add(_addDto);
        }

        [Then("دسته بندی  با عنوان 'لبنیات' در فهرست دسته بندی باید وجود داشته باشد")]
        public void Then()
        {
            var expected = _dataContext.Categories.FirstOrDefault();
            expected.Name.Should().Be(_addDto.Name);
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
