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
using System.Linq;
using Xunit;
using static SuperMarket.Specs.BDDHelper;

namespace SuperMarket.Specs.Categories
{
    [Scenario("تعریف دسته بندی")]
    [Feature("",
          AsA = "فروشنده ",
          IWantTo = "دسته بندی را ویرایش کنم",
          InOrderTo = " دسته بندی ها تغییر ایجاد کنم"
          )]
    public class UpdateCategoryWithDuplicateName : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly CategoryService _sut;
        private readonly UnitOfWork _unitOfWork;
        private readonly CategoryRepository _categoryRepository;
        private Category _category1;
        private Category _category2;
        private UpdateCategoryDto _dto;
        Action expected;

        public UpdateCategoryWithDuplicateName(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _categoryRepository = new EFCategoryRepository(_dataContext);
            _sut = new CategoryAppService(_categoryRepository, _unitOfWork);
        }

        [Given("دسته بندی با عنوان 'لبنیات' و 'خشکبار' در فهرست دسته بندی وجود دارد")]
        public void Given()
        {

            _category1 = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(_category1));

            _category2 = CategoryFactory.CreateCategory("خشکبار");
            _dataContext.Manipulate(_ => _.Categories.Add(_category2));


        }

        [When("دسته بندی 'خشکبار' را به 'لبنیات' تغییر میدهیم")]
        public void When()
        {
            _dto = new UpdateCategoryDto
            {
                Name = "لبنیات"
            };

            expected = () => _sut.Update(_dto, _category2.Id);
        }

        [Then("تنها یک دسته بندی با عنوان ' لبنیات' باید در فهرست دسته بندی کالا وجود داشته باشد")]
        public void Then()
        {
            _dataContext.Categories.Where(p => p.Name == _dto.Name)
                .Should().HaveCount(1);
        }

        [And("خطایی با عنوان 'عنوان دسته بندی کالا وجود دارد' باید رخ دهد")]
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
