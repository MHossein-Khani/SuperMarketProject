using FluentAssertions;
using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using SuperMarket.Infrastructure.Test;
using SuperMarket.Persistance.EF;
using SuperMarket.Persistance.EF.Products;
using SuperMarket.Persistance.EF.SalesInvoices;
using SuperMarket.Services.Products;
using SuperMarket.Services.Products.Cantracts;
using SuperMarket.Services.SalesInvoices.Contracts;
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
           IWantTo = "کالا را تعریف کنم",
           InOrderTo = " در فاکتور ها از آن استفاده کنم"
           )]
    public class AddProduct : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly UnitOfWork _unitOfWork;
        private readonly ProductRepository _productRipository;
        private readonly SalesInvoiceRepository _salesInvoiceRepository;
        private readonly ProductService _sut;
        private Category _category;
        private AddProductDto _dto;
       
        public AddProduct(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _productRipository = new EFProductRepository(_dataContext);
            _salesInvoiceRepository = new EFSalesInvoiceRepository(_dataContext);
            _sut = new ProductAppService(_productRipository, _salesInvoiceRepository, _unitOfWork);
        }

        [Given("دسته بندی با عنوان 'لبنیات' در فهرست دسته بندی وجود دارد")]
        public void Given()
        {
            _category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(_category));
        }

        [And("هیچ کالایی در دسته بندی با عنوان 'لبنیات' وجود ندارد")]
        public void And()
        {
        }

        [When("کالایی با کد '1' با عنوان 'شیر کاله' " +
            "با حداقل موجودی '5' با قیمت فروش '5000' " +
            "با موجودی '10' در دسته بندی 'لبنیات' تعریف میکنیم")]
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
            _sut.Add(_dto);
        }

        [Then("کالایی با کد '1' با عنوان 'شیر کاله'" +
            " با حداقل موجودی '5' با قیمت فروش '5000' " +
            "با موجودی '10' در دسته بندی 'لبنیات' باید وجود داشته باشد")]
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
