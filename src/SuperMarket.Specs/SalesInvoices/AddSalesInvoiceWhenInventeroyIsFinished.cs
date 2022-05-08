using FluentAssertions;
using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using SuperMarket.Infrastructure.Test;
using SuperMarket.Persistance.EF;
using SuperMarket.Persistance.EF.SalesInvoices;
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
    public class AddSalesInvoiceWhenInventeroyIsFinished : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly UnitOfWork _unitOfWork;
        private readonly SalesInvoiceRepository _ripository;
        private readonly SalesInvoiceService _sut;
        private Category _category;
        private Product _product;
        private AddSalesInvoiceDto _dto;
        Action expected;

        public AddSalesInvoiceWhenInventeroyIsFinished(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _ripository = new EFSalesInvoiceRepository(_dataContext);
            _sut = new SalesInvoiceAppService(_ripository, _unitOfWork);
        }

        [Given("کالایی با کد '1' با عنوان 'شیر کاله'  " +
            "با حداقل موجودی'5' با قیمت فروش '5000' " +
            "با موجودی '0' در دسته بندی 'لبنیات' وجود دارد")]
        public void Given()
        {
            _category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(_category));

            _product = ProductFactory.CreatProduct("1", 0, _category.Id);
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
                Date = new DateTime(05 / 02 / 2022),
                ProductId = _product.Id,
            };
            expected = () => _sut.Add(_dto);
        }

        [Then("هیچ فاکتور فروشی در فهرست فاکتورهای فروش نباید وجود داشته باشد")]
        public void Then()
        {
            _dataContext.SalesInvoices.Should().HaveCount(0);
        }

        [And("خطایی با عنوان ' موجودی کالا به اتمام رسیده است' باید رخ دهد")]
        public void AndThen()
        {
            expected.Should().ThrowExactly<ProductInventoryIsFinishedException>();
        }

        [Fact]
        public void Run()
        {
            Given();
            And();
            When();
            Then();
            AndThen();
        }
    }
}
