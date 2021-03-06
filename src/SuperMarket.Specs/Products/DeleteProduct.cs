using FluentAssertions;
using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using SuperMarket.Infrastructure.Test;
using SuperMarket.Persistance.EF;
using SuperMarket.Persistance.EF.Products;
using SuperMarket.Persistance.EF.SalesInvoices;
using SuperMarket.Services.Products;
using SuperMarket.Services.Products.Cantracts;
using SuperMarket.Services.SalesInvoices.Contracts;
using SuperMarket.Specs.Infrastructure;
using SuperMarket.Test.Tools;
using System;
using Xunit;
using static SuperMarket.Specs.BDDHelper;

namespace SuperMarket.Specs.Products
{
    public class DeleteProduct : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly UnitOfWork _unitOfWork;
        private readonly ProductRepository _productRipository;
        private readonly SalesInvoiceRepository _salesInvoiceRepository;
        private readonly ProductService _sut;
        private Category _category;
        private Product _product;
               
        public DeleteProduct(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _productRipository = new EFProductRepository(_dataContext);
            _salesInvoiceRepository = new EFSalesInvoiceRepository(_dataContext);
            _sut = new ProductAppService(_productRipository, _salesInvoiceRepository,_unitOfWork);
        }

        [Given("دسته بندی با عنوان 'لبنیات' در فهرست دسته بندی وجود دارد")]
        public void Given()
        {
            _category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(_category));
        }

        [And("کالای با کد '1' در دسته بندی 'لبنیات' وجود دارد")]
        public void GivenAnd()
        {
            _product = ProductFactory.CreatProduct("1", 10, _category.Id);
            _dataContext.Manipulate(_ => _.products.Add(_product));
        }

        [And("کالای با کد '1' در فاکتور فروش استفاده نشده باشد")]
        public void And()
        {

        }

        [When("کالا با کد '1' با عنوان 'شیر کاله' " +
            "با حداقل موجودی '5' با قیمت فروش '5000' " +
            "با موجودی '10' در دسته بندی 'لبنیات' را حذف میکنیم")]
        public void When()
        {
            _sut.Delete(_product.Id);
        }

        [Then("کالا با کد '1' با عنوان 'شیر کاله' " +
            "با حداقل موجودی '5' با قیمت فروش '5000' " +
            "با موجودی '10' در دسته بندی 'لبنیات' نباید وجود داشته باشد")]
        public void Then()
        {
            _dataContext.products.Should().HaveCount(0); 
        }

        [Fact]
        public void Run()
        {
            Given();
            GivenAnd();
            And();
            When();
            Then();
        }
    }
}
