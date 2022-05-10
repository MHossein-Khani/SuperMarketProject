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
using System.Collections.Generic;
using Xunit;
using static SuperMarket.Specs.BDDHelper;

namespace SuperMarket.Specs.PurchaseInvoices
{
    public class GetAllPurchaseInvoice : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly UnitOfWork _unitOfWork;
        private readonly PurchaseInvoiceRepository _ripository;
        private readonly ProductRepository _productRepository;
        private readonly PurchaseInvoiceService _sut;
        private Category _category;
        private Product _product1;
        private Product _product2;
        private List<PurchaseInvoice> _purchaseInvoices;
        private List<GetPurchaseInvoiceDto> _expected;

        public GetAllPurchaseInvoice(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _ripository = new EFPurchaseInvoiceRepository(_dataContext);
            _productRepository = new EFProductRepository(_dataContext);
            _sut = new PurchaseInvoiceAppService(_ripository, _productRepository, _unitOfWork);
        }

        [Given("یک فاکتور خرید با کد کالا ‘1’ " +
            "با نام کالا ‘شیر کاله’ ‘ با تعداد’2’ با قیمت واحد’5000’  " +
            "با تاریخ ‘2022/02/05’ و به فاکتور خرید با کد کالا ‘2’ " +
            "با نام کالا ‘ ماست سون ‘  با تعداد’2’ " +
            " با قیمت واحد’7000’ با تاریخ ‘2022/02/06’ وجود دارد")]
        public void Given()
        {
            _category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(_category));

            _product1 = ProductFactory.CreatProduct("1", 10, _category.Id);
            _dataContext.Manipulate(_ => _.products.Add(_product1));

            _product2 = ProductFactory.CreatProduct("2", 5, _category.Id);
            _dataContext.Manipulate(_ => _.products.Add(_product2));
            Generate_a_list_of_purchase_invoices();
        }

        [When("درخواست مشاهده فاکتورهای خرید را بدهیم")]
        public void When()
        {
            _expected = _sut.GetAll();
        }

        [Then(":  یک فاکتور خرید با کد کالا ‘1’ " +
            "با نام کالا ‘شیر کاله’ ‘ با تعداد’2’ با قیمت واحد’5000’  " +
            "با تاریخ ‘2022/02/05’ و به فاکتور خرید با کد کالا ‘2’ " +
            "با نام کالا ‘ ماست سون ‘  با تعداد’2’  با قیمت واحد’7000’ " +
            "با تاریخ ‘2022/02/06’ باید نشان داده شوند")]
        public void Then()
        {
            _expected.Should().HaveCount(2);
        }

        [Fact]
        public void Run()
        {
            Given();
            When();
            Then();
        }

        private void Generate_a_list_of_purchase_invoices()
        {
            var purchaseInvoices = new List<PurchaseInvoice>
            {
                new PurchaseInvoice
                {
                CodeOfProduct = _product1.Code,
                NameOfProduct = _product1.Name,
                Number = 2,
                Price = 10000,
                Date = new DateTime(05 / 02 / 2022),
                ProductId = _product1.Id,
                },

                new PurchaseInvoice
                {
                CodeOfProduct = _product2.Code,
                NameOfProduct = _product2.Name,
                Number = 2,
                Price = 10000,
                Date = new DateTime(07 / 02 / 2022),
                ProductId = _product2.Id,
                }
            };
            _dataContext.Manipulate(_ => _.PurchaseInvoices.AddRange(purchaseInvoices));
        }

    }
}
