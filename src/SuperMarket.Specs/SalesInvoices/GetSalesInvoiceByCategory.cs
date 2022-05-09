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
    public class GetSalesInvoiceByCategory : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly UnitOfWork _unitOfWork;
        private readonly SalesInvoiceRepository _ripository;
        private readonly SalesInvoiceService _sut;
        private Category _category;
        private Product _product1;
        private Product _product2;
        private List<SalesInvoice> _salesInvoices;
        private List<GetSalesInvoiceDto> _expected;

        public GetSalesInvoiceByCategory(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _ripository = new EFSalesInvoiceRepository(_dataContext);
            _sut = new SalesInvoiceAppService(_ripository, _unitOfWork);
        }

        [Given(" فاکتور فروش  " +
            "با کد کالا ‘1’ با نام کالا ‘شیر کاله با تعداد’2’ با قیمت واحد’5000’" +
            " با قیمت کل ‘10000’ با تاریخ ‘2022/02/05’ با نام مشتری ‘حسین خانی’  و " +
            " فاکتور فروش با کد کالا ‘2’ با نام کالا ‘ ماست سون ‘  با تعداد’2’  " +
            "با قیمت واحد’7000’ با قیمت کل ‘14000’ با تاریخ ‘2022/02/06’ " +
            "با نام مشتری ‘ابراهیم رمضانی’در فهرست فاکتورهای فروش وجود دارند")]
        public void Given()
        {
            _category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(_category));

            _product1 = ProductFactory.CreatProduct("1", 8, _category.Id);
            _dataContext.Manipulate(_ => _.products.Add(_product1));

            _product2 = ProductFactory.CreatProduct("2", 10, _category.Id);
            _dataContext.Manipulate(_ => _.products.Add(_product2));

            Generate_a_list_of_salesInvoice();
        }


        [When(" درخواست مشاهده فاکتورهای فروش در دسته بندی ‘لبنیات’ را میدهم")]
        public void when()
        {
            _expected = _sut.GetByCategory(_category.Id);
        }

        [Then(" فاکتور فروش  با کد کالا ‘1’ " +
            "با نام کالا ‘شیر کاله " +
            "با تعداد’2’ با قیمت واحد’5000’ " +
            "با قیمت کل ‘10000’ با تاریخ ‘2022/02/05’ " +
            "با نام مشتری ‘حسین خانی’  و " +
            " فاکتور فروش با کد کالا ‘2’ با نام کالا ‘ ماست سون ‘ " +
            " با تعداد’2’  با قیمت واحد’7000’ با قیمت کل ‘14000’ با تاریخ ‘2022/02/06’ " +
            "با نام مشتری ‘ابراهیم رمضانی’در فهرست فاکتورهای فروش باید نمایش داده شوند")]
        public void Then()
        {
            _expected.Should().HaveCount(2);
        }

        [Fact]
        public void Run()
        {
            Given();
            when();
            Then();
        }

        private void Generate_a_list_of_salesInvoice()
        {
            _salesInvoices = new List<SalesInvoice>()
            {
                new SalesInvoice()
                {
                    CodeOfProduct = _product1.Code,
                    NameOfProduct = _product1.Name,
                    Number = 2,
                    TotalCost = 10000,
                    Date = new DateTime(05/02/2022),
                    ProductId = _product1.Id,
                },
                new SalesInvoice()
                {
                    CodeOfProduct = _product2.Code,
                    NameOfProduct = _product2.Name,
                    Number = 2,
                    TotalCost = 15000,
                    Date = new DateTime(05/02/2022),
                    ProductId = _product2.Id,
                }
            };
            _dataContext.Manipulate(_ => _.SalesInvoices.AddRange(_salesInvoices));
        }

    }
}
