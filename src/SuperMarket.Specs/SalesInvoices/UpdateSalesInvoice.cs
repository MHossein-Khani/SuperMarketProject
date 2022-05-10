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
using SuperMarket.Specs.Infrastructure;
using SuperMarket.Test.Tools;
using System;
using System.Linq;
using Xunit;
using static SuperMarket.Specs.BDDHelper;

namespace SuperMarket.Specs.SalesInvoices
{
    [Scenario(".ویرایش فاکتور فروش")]
    [Feature("",
           AsA = "فروشنده ",
           IWantTo = "فاکتور فروش را ویرایش کنم",
           InOrderTo = " بتوانم در فاکتور فروش تغییرات ایجاد کنم"
           )]
    public class UpdateSalesInvoice : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly UnitOfWork _unitOfWork;
        private readonly SalesInvoiceRepository _ripository;
        private readonly ProductRepository _productRepository;
        private readonly SalesInvoiceService _sut;
        private Category _category;
        private Product _product1;
        private Product _product2;
        private SalesInvoice _salesInvoice;
        private UpdateSalesInvoiceDto _dto;

        public UpdateSalesInvoice(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _ripository = new EFSalesInvoiceRepository(_dataContext);
            _productRepository = new EFProductRepository(_dataContext);
            _sut = new SalesInvoiceAppService(_ripository, _productRepository, _unitOfWork);
        }

        [When("کالایی با کد '1' با عنوان 'شیر کاله' " +
            "با حداقل موجودی '5' با قیمت فروش '5000' " +
            "با موجودی '8' در دسته بندی 'لبنیات' وجود دارد")]
        public void Given()
        {
            _category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(_category));

            _product1 = ProductFactory.CreatProduct("1", 8, _category.Id);
            _dataContext.Manipulate(_ => _.products.Add(_product1));
        }

        [And("یک فاکتور فروش با " +
            "کد کالا '1' با نام کالا 'شیر کاله' " +
            " با تعداد'2' با قیمت واحد'5000' " +
            "با قیمت کل '10000'  با تاریخ '2022/02/05' " +
            "با نام مشتری 'حسین خانی' در فهرست فاکتورهای فروش وجود دار")]
        public void GivenAnd()
        {
            _salesInvoice = SalesInvoiceFactory.
                CreateSalesInvoice(_product1.Code, _product1.Name, _product1.Id);
            _dataContext.Manipulate(_ => _.SalesInvoices.Add(_salesInvoice));
        }

        [And("کالایی با کد '2'  ' " +
            " در دسته بندی 'لبنیات' وجود دارد")]
        public void And()
        {
            _product2 = ProductFactory.CreatProduct("2", 10, _category.Id);
            _dataContext.Manipulate(_ => _.products.Add(_product2));
        }

        [When("فاکتور فروش با کد کالا '1' به کد کالای '2' تغییر دهیم")]
        public void When()
        {
            _dto = new UpdateSalesInvoiceDto
            {
                CodeOfProduct = _product2.Code,
                NameOfProduct = _product2.Name,
                Number = 2,
                TotalCost = 15000,
                Date = new DateTime(05 / 02 / 2022),
                ProductId = _product2.Id,
            };
            _sut.Update(_dto, _salesInvoice.Id);
        }

        [Then("فاکتور فروش با کد کالا '1' به کد کالای '2' تغییر پیدا کرده است")]
        public void Then()
        {
            var expected = _dataContext.SalesInvoices.FirstOrDefault();
            expected.CodeOfProduct.Should().Be(_dto.CodeOfProduct);
        }

        [And("کالا با کد '1' با موجودی '10' و کالا با کد کالای '2' " +
            "با موجودی '8' باید در فهرست کالا وجود داشته باشد ")]
        public void ThenAnd()
        {
            _product1.Inventory.Should().Be(10);
            _product2.Inventory.Should().Be(8);
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
