using FluentAssertions;
using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using SuperMarket.Infrastructure.Test;
using SuperMarket.Persistance.EF;
using SuperMarket.Persistance.EF.Products;
using SuperMarket.Persistance.EF.PurchaseInvoices;
using SuperMarket.Services.Products;
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
    [Scenario("خرید کالا")]
    [Feature("",
           AsA = "فروشنده ",
           IWantTo = "خرید کالا داشته باشم ",
           InOrderTo = " بتوانم موجودی کالای خود را افزایش دهم"
           )]
    public class AddPurchaseInvoice : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly UnitOfWork _unitOfWork;
        private readonly PurchaseInvoiceRepository _ripository;
        private readonly ProductRepository _productRepository;
        private readonly PurchaseInvoiceService _sut;
        private Category _category;
        private Product _product;
        private AddPurchaseInvoiceDto _dto;

        public AddPurchaseInvoice(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _ripository = new EFPurchaseInvoiceRepository(_dataContext);
            _productRepository = new EFProductRepository(_dataContext);
            _sut = new PurchaseInvoiceAppService(_ripository, _productRepository, _unitOfWork);
        }

        [Given("کالایی با کد '1' با عنوان 'شیر کاله'  " +
            "با حداقل موجودی'5' با قیمت فروش '5000' " +
            "با موجودی '10' در دسته بندی 'لبنیات' وجود دارد")]
        public void Given()
        {
            _category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(_category));

            _product = ProductFactory.CreatProduct("1", 10, _category.Id);
            _dataContext.Manipulate(_ => _.products.Add(_product));
        }

        [And(" هیچ فاکتور فروشی در فهرست فاکتورهای فروش وجود ندارد")]
        public void And()
        {
        }

        [When(" کالا با کد کالا ‘1’ با نام کالا ‘شیر کاله’ " +
            "با تعداد ‘4’ با قیمت واحد’5000’  " +
            "با تاریخ ‘2022/02/05’  خریداری میشود")]
        public void When()
        {
            _dto = new AddPurchaseInvoiceDto()
            {
                CodeOfProduct = _product.Code,
                NameOfProduct = _product.Name,
                Number = 2,
                Price = 10000,
                Date = new DateTime(05 / 05 / 2022),
                ProductId = _product.Id,
            };
            _sut.Add(_dto);
        }

        [Then("یک فاکتور خرید کالا با کد کالا ‘1’ " +
            "با نام کالا ‘شیر کاله’ با تعداد ‘4’ " +
            "با قیمت واحد’5000’  با تاریخ ‘2022/02/05’   " +
            "در فهرست فاکتورهای خرید باید وجود داشته باشد")]
        public void Then()
        {
            var expected = _dataContext.PurchaseInvoices.FirstOrDefault();
            expected.Should().NotBeNull();
            expected.CodeOfProduct.Should().Be(_dto.CodeOfProduct);
            expected.NameOfProduct.Should().Be(_dto.NameOfProduct);
            expected.Number.Should().Be(_dto.Number);
            expected.Price.Should().Be(_dto.Price);
            expected.Date.Should().Be(_dto.Date);
            expected.ProductId.Should().Be(_dto.ProductId);
        }

        [And("کالا با کد ‘1’ " +
            "با عنوان ‘شیر کاله’ با حداقل موجودی ‘5’ " +
            "با قیمت فروش ‘5000’ با موجودی ‘10’ " +
            "در دسته بندی ‘لبنیات’ باید وجود داشته باشد")]
        public void ThenAnd()
        {
            _product.Inventory.Should().Be(12);
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
