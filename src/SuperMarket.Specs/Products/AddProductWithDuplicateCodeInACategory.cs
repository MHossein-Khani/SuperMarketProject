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
           IWantTo = "کالا را تعریف کنم",
           InOrderTo = " در فاکتور ها از آن استفاده کنم"
           )]
    public class AddProductWithDuplicateCodeInACategory : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly UnitOfWork _unitOfWork;
        private readonly ProductRepository _productRipository;
        private readonly ProductService _sut;
        private Category _category;
        private AddProductDto _dto;
        Action expected;

        public AddProductWithDuplicateCodeInACategory(ConfigurationFixture configuration) : base(configuration)
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

        [And(" کالایی با کد '1' با عنوان 'شیر کاله' " +
            "با حداقل موجودی '5' با قیمت فروش '5000' " +
            "با موجودی '10' در دسته بندی 'لبنیات' وجود دارد")]
        public void And()
        {
            var product = ProductFactory.CreatProduct(_category.Id);
            _dataContext.Manipulate(_ => _.products.Add(product));
        }

        [When("کالایی با کد '1'  تعریف میکنیم")]
        public void When()
        {
            _dto = new AddProductDto
            {
                Code = "1",
                Name = "شیر کاله",
                MinimumInventory = 5,
                Price = 5000,
                Inventory = 10,
                CategoryId = _category.Id
            };
            expected = () => _sut.Add(_dto);
        }

        [Then(" تنها یک کالا با کد '1' باید وجود داشته باشد")]
        public void Then()
        {
            _dataContext.products.Should().HaveCount(1);
        }

        [And("خطایی با عنوان ' این کالا در این دسته بندی تعریف شده است' باید رخ دهد")]
        public void ThenAnd()
        {
            expected.Should().ThrowExactly<ProductISAlreadyExistException>();
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
