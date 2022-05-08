using FluentAssertions;
using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using SuperMarket.Infrastructure.Test;
using SuperMarket.Persistance.EF;
using SuperMarket.Persistance.EF.Products;
using SuperMarket.Services.Products;
using SuperMarket.Services.Products.Cantracts;
using SuperMarket.Specs.Infrastructure;
using SuperMarket.Test.Tools;
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
    public class UpdateProduct : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly UnitOfWork _unitOfWork;
        private readonly ProductRepository _productRipository;
        private readonly ProductService _sut;
        private Category _category;
        private Product _product;
        private UpdateProductDto _dto;

        public UpdateProduct(ConfigurationFixture configuration) : base(configuration)
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
            _product = ProductFactory.CreatProduct("1", _category.Id);
            _dataContext.Manipulate(_ => _.products.Add(_product));
        }

        [When("کالای با کد '1' با عنوان 'شیر کاله' با حداقل موجودی '5' " +
            "با قیمت فروش'5000' با موجودی '10' را به  کالای با کد '2' " +
            "با عنوان 'ماست سون' با حداقل موجودی '2' با قیمت فروش '7000' " +
            "با موجودی '5' را در دسته بندی 'لبنیات' تغییر میدهیم")]
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
            _sut.Update(_dto, _product.Id);
        }

        [Then(" کالای با کد '1' با عنوان 'شیر کاله' با حداقل موجودی '5' " +
            "با قیمت فروش '5000' با موجودی '10' باید  به  کالای با کد '2' " +
            "با عنوان 'ماست سون' با حداقل موجودی '2' با قیمت فروش '7000'" +
            " با موجودی '5'  در دسته بندی 'لبنیات'  تغییر پیدا کرده باش")]
        public void Then()
        {
            var expected = _dataContext.products.FirstOrDefault();
            expected.Code.Should().Be(_dto.Code);
            expected.Name.Should().Be(_dto.Name);
            expected.MinimumInventory.Should().Be(_dto.MinimumInventory);
            expected.Price.Should().Be(_dto.Price);
            expected.Inventory.Should().Be(_dto.Inventory);
            expected.CategoryId.Should().Be(_dto.CategoryId);
        }

        [Fact]
        public void Run()
        {
            Given();
            And();
            When();
            Then();
        }

    }
}
