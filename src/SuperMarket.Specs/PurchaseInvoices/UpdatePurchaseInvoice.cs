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
using System;
using System.Linq;
using Xunit;
using static SuperMarket.Specs.BDDHelper;

namespace SuperMarket.Specs.PurchaseInvoices
{

    [Scenario("ویرایش فاکتور خرید")]
    [Feature("",
           AsA = "فروشنده ",
           IWantTo = "فاکتور خرید را ویرایش کنم ",
           InOrderTo = " بتوانم در فاکتور خرید تغییرات ایجاد کنم"
           )]
    public class UpdatePurchaseInvoice : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly UnitOfWork _unitOfWork;
        private readonly PurchaseInvoiceRepository _ripository;
        private readonly ProductRepository _productRepository;
        private readonly PurchaseInvoiceService _sut;
        private Category _category;
        private Product _product1;
        private Product _product2;
        private PurchaseInvoice _purchaseInvoice;
        private UpdatePurchaseInvoiceDto _dto;

        public UpdatePurchaseInvoice(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _ripository = new EFPurchaseInvoiceRepository(_dataContext);
            _productRepository = new EFProductRepository(_dataContext);
            _sut = new PurchaseInvoiceAppService(_ripository, _productRepository, _unitOfWork);
        }

        [When("کالایی با کد ‘1’ با عنوان ‘شیر کاله’ " +
            "با حداقل موجودی ‘5’ با قیمت فروش ‘5000’ " +
            "با موجودی ‘10’ در دسته بندی ‘لبنیات’ وجود دارد")]
        public void Given()
        {
            _category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(_category));

            _product1 = ProductFactory.CreatProduct("1", 10, _category.Id);
            _dataContext.Manipulate(_ => _.products.Add(_product1));
        }

        [And("یک فاکتور خرید " +
            "با کد کالا ‘1’ با نام کالا ‘شیر کاله’ ‘ " +
            "با تعداد’2’ با قیمت واحد’5000’  " +
            "با تاریخ ‘2022/02/05’  در فهرست فاکتورهای خرید وجود دارد")]
        public void GivenAnd()
        {
            _purchaseInvoice = PurchaseInvoiceFactory.
                CreateSalesInvoice(_product1.Code, _product1.Name, _product1.Id);
            _dataContext.Manipulate(_ => _.PurchaseInvoices.Add(_purchaseInvoice));
        }

        [And("کالایی با کد ‘2’ با عنوان ‘ماست سون’ " +
            "با حداقل موجودی ‘2’ با قیمت فروش ‘7000’ " +
            "با موجودی ‘8’  در دسته بندی ‘لبنیات’  وجود دارد")]
        public void And()
        {
            _product2 = ProductFactory.CreatProduct("2", 8, _category.Id);
            _dataContext.Manipulate(_ => _.products.Add(_product2));
        }

        [When(":  فاکتور خرید با کد کالا ‘1’ با نام کالا ‘شیر کاله’ " +
            " با تعداد’2’ با قیمت واحد’5000’  " +
            "با تاریخ ‘2022/02/05’  را به فاکتور خرید " +
            "با کد کالا ‘2’ با نام کالا ‘ ماست سون ‘  " +
            "با تعداد’2’  با قیمت واحد’7000’ " +
            "با تاریخ ‘2022/02/06’ تغییر میدهیم")]
        public void When()
        {
            _dto = new UpdatePurchaseInvoiceDto
            {
                CodeOfProduct = _product2.Code,
                NameOfProduct = _product2.Name,
                Number = 2,
                Price = 7000,
                Date = new DateTime(05 / 02 / 2022),
                ProductId = _product2.Id,
            };
            _sut.Update(_dto, _purchaseInvoice.Id);
        }

        [Then("فاکتور خرید" +
            " با کد کالا ‘1’ با نام کالا ‘شیر کاله’  " +
            "با تعداد’2’ با قیمت واحد’5000’  " +
            "با تاریخ ‘2022/02/05’  را به فاکتور خرید " +
            "با کد کالا ‘2’ با نام کالا ‘ ماست سون ‘  " +
            "با تعداد’2’  با قیمت واحد’7000’ " +
            "با تاریخ ‘2022/02/06’   تغییر پیدا کرده باشد.")]
        public void Then()
        {
            var expected = _dataContext.PurchaseInvoices.FirstOrDefault();
            expected.CodeOfProduct.Should().Be(_dto.CodeOfProduct);
        }

        [And("کالا با کد ‘1’ با عنوان ‘شیر کاله’ " +
            "با حداقل موجودی ‘5’ با قیمت فروش ‘5000’ " +
            "با موجودی ‘8’ در دسته بندی ‘لبنیات’ باید  وجود داشته باشد")]
        public void ThenAnd()
        {
            _product1.Inventory.Should().Be(8);
            _product2.Inventory.Should().Be(10);
        }


        [Fact]
        public void Run()
        {
            Given();
            GivenAnd();
            And();
            When();
            Then();
            ThenAnd();
        }
    }
}
