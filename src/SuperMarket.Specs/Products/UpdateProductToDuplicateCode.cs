using FluentAssertions;
using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using SuperMarket.Infrastructure.Test;
using SuperMarket.Persistance.EF;
using SuperMarket.Persistance.EF.Products;
using SuperMarket.Services.Products;
using SuperMarket.Services.Products.Cantracts;
using SuperMarket.Services.Products.Exceptions;
using SuperMarket.Specs.Infrastructure;
using SuperMarket.Test.Tools;
using System;
using System.Linq;
using Xunit;
using static SuperMarket.Specs.BDDHelper;

namespace SuperMarket.Specs.Products
{
    [Scenario("تعریف کالا")]
    [Feature("",
          AsA = "فروشنده ",
          IWantTo = "کالا را ویرایش کنم",
          InOrderTo = " در آن تغییرات اعمال کنم"
          )]
    public class UpdateProductToDuplicateCode : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly UnitOfWork _unitOfWork;
        private readonly ProductRepository _productRipository;
        private readonly ProductService _sut;
        private Category _category;
        private Product _product1;
        private Product _product2;
        private UpdateProductDto _dto;
        Action expected;

        public UpdateProductToDuplicateCode(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _productRipository = new EFProductRepository(_dataContext);
            _sut = new ProductAppService(_productRipository, _unitOfWork);
        }

        [Given("دسته بندی با عنوان 'لبنیات' در فهرست دسته بندی وجود دارد")]
        public void Given()
        {
            _category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(_category));
        }

        [And("کالای با کد '1' و '2' در دسته بندی 'لبنیات' وجود دارد")]
        public void And()
        {
            _product1 = ProductFactory.CreatProduct("1", 10, _category.Id);
            _dataContext.Manipulate(_ => _.products.Add(_product1));

            _product2 = ProductFactory.CreatProduct("2", 10, _category.Id);
            _dataContext.Manipulate(_ => _.products.Add(_product2));
        }

        [When("کالای با کد '1' را به کالای با کد '2' را در دسته بندی 'لبنیات' تغییر میدهیم")]
        public void When()
        {
            _dto = new UpdateProductDto
            {
                Code = "2",
                Name = "ماست سون",
                MinimumInventory = 2,
                Price = 7000,
                Inventory = 5,
                CategoryId = _category.Id
            };

            expected = () => _sut.Update(_dto, _category.Id);
        }

        [Then("تنها یک کالا با کد '2 باید وجود داشته باشد'")]
        public void Then()
        {
            int count = _dataContext.products.Where(p => p.Code == "2").Count();
            count.Should().Be(1);
        }

        [And("خطایی با عنوان ' کالا در این دسته بندی تکراریست' باید رخ دهد")]
        public void ThenAnd()
        {
            expected.Should().ThrowExactly<CategoryCodeIsAlreadyExistException>();
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
