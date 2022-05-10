using FluentAssertions;
using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using SuperMarket.Infrastructure.Test;
using SuperMarket.Persistance.EF;
using SuperMarket.Persistance.EF.Products;
using SuperMarket.Persistance.EF.PurchaseInvoices;
using SuperMarket.Services.Products.Cantracts;
using SuperMarket.Services.PurchaseInvoices;
using SuperMarket.Services.PurchaseInvoices.Contracts;
using SuperMarket.Specs.Infrastructure;
using SuperMarket.Test.Tools;
using Xunit;
using static SuperMarket.Specs.BDDHelper;

namespace SuperMarket.Specs.PurchaseInvoices
{
    [Scenario(".حذف فاکتور خرید")]
    [Feature("",
           AsA = "فروشنده ",
           IWantTo = "فاکتور خرید را حذف کنم",
           InOrderTo = " در فاکتور خرید در محاسبه سود و زیان وجود نداشته باشد"
           )]
    public class DeletePurchaseInvoice : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly UnitOfWork _unitOfWork;
        private readonly PurchaseInvoiceRepository _ripository;
        private readonly ProductRepository _productRepository;
        private readonly PurchaseInvoiceService _sut;
        private Category _category;
        private Product _product;
        private PurchaseInvoice _purchaseInvoice;

        public DeletePurchaseInvoice(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _ripository = new EFPurchaseInvoiceRepository(_dataContext);
            _productRepository = new EFProductRepository(_dataContext);
            _sut = new PurchaseInvoiceAppService(_ripository, _productRepository, _unitOfWork);
        }

        [Given("یک فاکتور خرید " +
            "با کد کالا ‘1’ با نام کالا ‘شیر کاله’ ‘ " +
            "با تعداد’2’ با قیمت واحد’5000’  " +
            "با تاریخ ‘2022/02/05’ و به فاکتور خرید " +
            "با کد کالا ‘2’ با نام کالا ‘ ماست سون ‘ " +
            " با تعداد’2’  با قیمت واحد’7000’ " +
            "با تاریخ ‘2022/02/06’ وجود دارد")]
        public void Given()
        {
            _category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(_category));

            _product = ProductFactory.CreatProduct("1", 10, _category.Id);
            _dataContext.Manipulate(_ => _.products.Add(_product));

            _purchaseInvoice = PurchaseInvoiceFactory.
                CreateSalesInvoice(_product.Code, _product.Name, _product.Id);
            _dataContext.Manipulate(_ => _.PurchaseInvoices.Add(_purchaseInvoice));
        }

        [When("فاکتور خرید با کد کالا ‘1’ " +
            "با نام کالا ‘شیر کاله’ ‘ " +
            "با تعداد’2’ با قیمت واحد’5000’  " +
            "با تاریخ ‘2022/02/05’را حذف میکنیم")]
        public void When()
        {
            _sut.Delete(_purchaseInvoice.Id);
        }

        [Then(": فاکتور خرید با کد کالا ‘1’ " +
            "با نام کالا ‘شیر کاله’ ‘ " +
            "با تعداد’2’ با قیمت واحد’5000’  " +
            "با تاریخ ‘2022/02/05’ نباید وجود داشته باشد")]
        public void Then()
        {
            _dataContext.SalesInvoices.Should().HaveCount(0);
        }

        [And("کالا با کد '1' با عنوان با موجودی '10' باید وجود داشته باشد")]
        public void ThenAnd()
        {
            _product.Inventory.Should().Be(8);
        }

        [Fact]
        public void Run()
        {
            Given();
            When();
            Then();
            ThenAnd();
        }
    }
}
