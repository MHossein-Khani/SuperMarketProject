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
using System.Collections.Generic;
using System.Linq;
using Xunit;
using static SuperMarket.Specs.BDDHelper;

namespace SuperMarket.Specs.SalesInvoices
{
    [Scenario("نمایش فاکتور فروش")]
    [Feature("",
          AsA = "فروشنده ",
          IWantTo = "فاکتور فروش را مشاهده کنم",
          InOrderTo = " فروش براساس دسته بندی و کالا را مشاهده کنم"
          )]
    public class GetSalesInvoiceByProduct : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly UnitOfWork _unitOfWork;
        private readonly SalesInvoiceRepository _ripository;
        private readonly SalesInvoiceService _sut;
        private Category _category;
        private Product _product;
        private List<SalesInvoice> _salesInvoices;
        private List<GetSalesInvoiceDto> _expected;
        public GetSalesInvoiceByProduct(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _ripository = new EFSalesInvoiceRepository(_dataContext);
            _sut = new SalesInvoiceAppService(_ripository, _unitOfWork);
        }

        [Given("فاکتور فروش  " +
            "با کد کالا ‘1’ با نام کالا ‘شیر کاله با تعداد’2’ با قیمت واحد’5000’ " +
            "با قیمت کل ‘10000’ با تاریخ ‘2022/02/05’ با نام مشتری ‘حسین خانی’  " +
            "و  فاکتور فروش با کد کالا ‘1’ با نام کالا ‘ ماست سون ‘  با تعداد’4’  " +
            "با قیمت واحد’5000’ با قیمت کل ‘20000’ با تاریخ ‘2022/02/06’ " +
            "با نام مشتری ‘ابراهیم رمضانی’در فهرست فاکتورهای فروش وجود دارند")]

        public void Given()
        {
            _category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(_category));

            _product = ProductFactory.CreatProduct("1", 15, _category.Id);
            _dataContext.Manipulate(_ => _.products.Add(_product));
            Create_a_list_of_sales_invoice();
        }

        private void Create_a_list_of_sales_invoice()
        {
            var salesInvoice = new List<SalesInvoice>
            {
                new SalesInvoice
                {
                CodeOfProduct = _product.Code,
                NameOfProduct = _product.Name,
                Number = 2,
                TotalCost = 10000,
                Date = new DateTime(05 / 02 / 2022),
                ProductId = _product.Id,
                },

                new SalesInvoice
                {
                CodeOfProduct = _product.Code,
                NameOfProduct = _product.Name,
                Number = 4,
                TotalCost = 10000,
                Date = new DateTime(05 / 02 / 2022),
                ProductId = _product.Id,
                },
            };
            _dataContext.Manipulate(_ => _.SalesInvoices.AddRange(salesInvoice));
        }

        [When("درخواست مشاهده فاکتورهای فروش کالا با کد ‘1’ را میدهم")]
        public void When()
        {
            _expected = _sut.GetByProduct(_product.Id);
        }

        [Then("فاکتور فروش  با کد کالا ‘1’ با نام کالا ‘شیر کاله با تعداد’2’ " +
            "با قیمت واحد’5000’ با قیمت کل ‘10000’ با تاریخ ‘2022/02/05’ " +
            "با نام مشتری ‘حسین خانی’  و  فاکتور فروش با کد کالا ‘1’ " +
            "با نام کالا ‘ ماست سون ‘  با تعداد’4’  با قیمت واحد’5000’ " +
            "با قیمت کل ‘20000’ با تاریخ ‘2022/02/06’ " +
            "با نام مشتری ‘ابراهیم رمضانی’در فهرست فاکتورهای فروش باید نمایش داده شوند")]
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
    }
}
