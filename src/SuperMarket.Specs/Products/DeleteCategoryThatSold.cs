using FluentAssertions;
using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using SuperMarket.Infrastructure.Test;
using SuperMarket.Persistance.EF;
using SuperMarket.Persistance.EF.Products;
using SuperMarket.Services.Products;
using SuperMarket.Services.Products.Cantracts;
using SuperMarket.Services.Products.Exceptions;
using SuperMarket.Specs.Infrastructure;
using SuperMarket.Test.Tools;
using System;
using System.Linq;
using Xunit;
using static SuperMarket.Specs.BDDHelper;

namespace SuperMarket.Specs.Products
{

    public class DeleteCategoryThatSold : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly UnitOfWork _unitOfWork;
        private readonly ProductRepository _productRipository;
        private readonly ProductService _sut;
        private Category _category;
        private Product _product;
        private SalesInvoice _salesInvoice;
        Action expected;

        public DeleteCategoryThatSold(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _productRipository = new EFProductRepository(_dataContext);
            _sut = new ProductAppService(_productRipository, _unitOfWork);
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
            _product = ProductFactory.CreatProduct("1", _category.Id);
            _dataContext.Manipulate(_ => _.products.Add(_product));
        }

        [And("کالای با کد '1' در فاکتور فروش استفاده شده باشد")]
        public void And()
        {
            _salesInvoice = SalesInvoiceFactory.
                CreateSalesInvoice(_product.Code, _product.Name, _product.Id);
            _dataContext.Manipulate(_ => _.SalesInvoices.Add(_salesInvoice));
        }

        [When("کالا با کد '1 " +
            " دسته بندی 'لبنیات' را حذف میکنیم")]
        public void When()
        {
            expected = () =>_sut.Delete(_product.Id);
        }

        [Then("کالا با کد '1' باید وجود داشته باشد")]
        public void Then()
        {
            _dataContext.products.Should().HaveCount(1);
        }
    }
}
