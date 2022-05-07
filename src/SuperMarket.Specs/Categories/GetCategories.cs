using FluentAssertions;
using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using SuperMarket.Infrastructure.Test;
using SuperMarket.Persistance.EF;
using SuperMarket.Persistance.EF.Categories;
using SuperMarket.Services.Categories;
using SuperMarket.Services.Categories.Contracts;
using SuperMarket.Specs.Infrastructure;
using System.Collections.Generic;
using Xunit;
using static SuperMarket.Specs.BDDHelper;

namespace SuperMarket.Specs.Categories
{
    [Scenario("نمایش دسته بندی")]
    [Feature("",
           AsA = "فروشنده ",
           IWantTo = "دسته بندی را نمایش دهم",
           InOrderTo = "دسته بندی ها را مشاهده کنم"
           )]
    public class GetCategories : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly CategoryService _sut;
        private readonly UnitOfWork _unitOfWork;
        private readonly CategoryRepository _categoryRepository;
        private List<Category> _categories;
        private List<GetCategoryDto> _expected;

        public GetCategories(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _categoryRepository = new EFCategoryRepository(_dataContext);
            _sut = new CategoryAppService(_categoryRepository, _unitOfWork);
        }

        [Given("دسته بندی هایی با عنوان 'لبنیات' و 'خشکبار' در فهرست دسته بندی وجود دارد")]
        public void Given()
        {
            _categories = new List<Category>
            {
                new Category
                {
                    Name = "لبنیات"
                },
                new Category
                {
                    Name = "خشکبار"
                }
            };
            _dataContext.Manipulate(_ => _.Categories.AddRange(_categories));
        }

        [When("درخواست نمایش فهرست دسته بندی را میدهیم")]
        public void When()
        {
            _expected = _sut.GetAll();
        }

        [Then("دسته بندی های 'لبنیات' و 'خشکبار' باید نمایش داده شده باشند")]
        public void Then()
        {
            _expected.Should().HaveCount(2);
            _expected.Should().Contain(p => p.Name == "لبنیات");
            _expected.Should().Contain(p => p.Name == "خشکبار");
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
