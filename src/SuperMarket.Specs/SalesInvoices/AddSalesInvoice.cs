using FluentAssertions;
using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using SuperMarket.Infrastructure.Test;
using SuperMarket.Persistance.EF;
using SuperMarket.Persistance.EF.SalesInvoices;
using SuperMarket.Services.SalesInvoices;
using SuperMarket.Services.SalesInvoices.Contracts;
using SuperMarket.Specs.Infrastructure;
using SuperMarket.Test.Tools;
using System;
using System.Linq;
using Xunit;
using static SuperMarket.Specs.BDDHelper;

namespace SuperMarket.Specs.SalesInvoices
{
    [Scenario("فروش کالا")]
    [Feature("",
           AsA = "فروشنده ",
           IWantTo = "کالا را به فروش برسانم",
           InOrderTo = " بتوانم سود و زیان را محاسبه کنم"
           )]
    public class AddSalesInvoice : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly UnitOfWork _unitOfWork;
        private readonly SalesInvoiceRepository _ripository;
        private readonly SalesInvoiceService _sut;
        private Category _category;
        private Product _product;
        private AddSalesInvoiceDto _dto;

        public AddSalesInvoice(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _ripository = new EFSalesInvoiceRepository(_dataContext);
            _sut = new SalesInvoiceAppService(_ripository, _unitOfWork);
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

        [When("یک کالا با کد کالا '1' " +
            "با نام کالا 'شیر کاله' با تعداد'2' با قیمت واحد'5000' " +
            "با قیمت کل '10000'  با تاریخ '2022 / 02 / 05' " +
            "با نام مشتری 'حسین خانی' به فروش میرسد")]
        public void When()
        {
            _dto = new AddSalesInvoiceDto
            {
                CodeOfProduct = _product.Code,
                NameOfProduct = _product.Name,
                Number = 2,
                TotalCost = 10000,
                Date = new DateTime(05 / 02 / 2022),
                ProductId = _product.Id,
            };
            _sut.Add(_dto);
        }

        [Then(" یک فاکتور فروش با کد کالا '1' با نام کالا 'شیر کاله' " +
            "با تعداد'2' با قیمت واحد'5000' با قیمت کل '10000'  " +
            "با تاریخ '2022 / 02 / 05' با نام مشتری 'حسین خانی' " +
            "در فهرست فاکتورهای فروش باید وجود داشته باشد")]
        public void Then()
        {
            var expected = _dataContext.SalesInvoices.FirstOrDefault();
            expected.Should().NotBeNull();
            expected.CodeOfProduct.Should().Be(_dto.CodeOfProduct);
            expected.NameOfProduct.Should().Be(_dto.NameOfProduct);
            expected.Number.Should().Be(_dto.Number);
            expected.Date.Should().Be(_dto.Date);
            expected.ProductId.Should().Be(_dto.ProductId);

        }

        [And("کالا با کد '1' " +
            "با عنوان 'شیر کاله' با حداقل موجودی '5' " +
            "با قیمت فروش '5000' با موجودی '8' " +
            "در دسته بندی 'لبنیات' باید وجود داشته باشد")]
        public void ThenAnd()
        {
            _product.Inventory -= _dto.Number;
            _dataContext.Manipulate(_ => _.products.Update(_product));
            var expectedProduct = _dataContext.products.FirstOrDefault();
            expectedProduct.Inventory.Should().Be(8);
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
