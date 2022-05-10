using FluentAssertions;
using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using SuperMarket.Infrastructure.Test;
using SuperMarket.Persistance.EF;
using SuperMarket.Persistance.EF.Products;
using SuperMarket.Persistance.EF.SalesInvoices;
using SuperMarket.Services.Products.Cantracts;
using SuperMarket.Services.SalesInvoices;
using SuperMarket.Services.SalesInvoices.Contracts;
using SuperMarket.Services.SalesInvoices.Exceptions;
using SuperMarket.Specs.Infrastructure;
using SuperMarket.Test.Tools;
using System;
using System.Linq;
using Xunit;
using static SuperMarket.Specs.BDDHelper;

namespace SuperMarket.Specs.SalesInvoices
{
    [Scenario(".حذف فاکتور فروش")]
    [Feature("",
           AsA = "فروشنده ",
           IWantTo = "فاکتور فروش را حذف کنم",
           InOrderTo = " بتوانم در فاکتور فروش در محاسبه سود و زیان وجود نداشته باشد"
           )]
    public class DeleteSalesInvoice : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly UnitOfWork _unitOfWork;
        private readonly SalesInvoiceRepository _ripository;
        private readonly ProductRepository _productRepository;
        private readonly SalesInvoiceService _sut;
        private Category _category;
        private Product _product;
        private SalesInvoice _salesInvoice;

        public DeleteSalesInvoice(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _ripository = new EFSalesInvoiceRepository(_dataContext);
            _productRepository = new EFProductRepository(_dataContext);
            _sut = new SalesInvoiceAppService(_ripository, _productRepository, _unitOfWork);
        }


        [Given("فرض میکنیم یک کالا یا کد '1 وجود دارد")]
        public void Given()
        {
            _category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(_category));

            _product = ProductFactory.CreatProduct("1", 8, _category.Id);
            _dataContext.Manipulate(_ => _.products.Add(_product));
        }

        [And("یک فاکتور فروش با کد کالا '1 وجود دارد")]
        public void GivenAnd()
        {
            _salesInvoice = SalesInvoiceFactory.
                CreateSalesInvoice(_product.Code, _product.Name, _product.Id);
            _dataContext.Manipulate(_ => _.SalesInvoices.Add(_salesInvoice));
        }

        [When("فاکتور فروش با کد کالای '1' را حذف میکنیم")]
        public void When()
        {
            _sut.Delete(_salesInvoice.Id);
        }

        [Then("فاکتور فروش با کد کالای '1' نباید وجود داشته باشد")]
        public void Then()
        {
            _dataContext.SalesInvoices.Should().HaveCount(0);
        }

        [And("کالا با کد '1' با عنوان با موجودی '10' باید وجود داشته باشد")]
        public void ThenAnd()
        {
            _product.Inventory.Should().Be(10);
        }

        [Fact]
        public void Run()
        {
            Given();
            GivenAnd();
            When();
            Then();
            ThenAnd();

        }
    }
}
