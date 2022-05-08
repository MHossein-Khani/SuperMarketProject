using FluentAssertions;
using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using SuperMarket.Infrastructure.Test;
using SuperMarket.Persistance.EF;
using SuperMarket.Persistance.EF.Products;
using SuperMarket.Services.Products;
using SuperMarket.Services.Products.Cantracts;
using SuperMarket.Specs.Infrastructure;
using SuperMarket.Test.Tools;
using System.Collections.Generic;
using Xunit;
using static SuperMarket.Specs.BDDHelper;

namespace SuperMarket.Specs.Products
{
    [Scenario("نمایش کالا")]
    [Feature("",
           AsA = "فروشنده ",
           IWantTo = "کالا را نمایش دهم",
           InOrderTo = "کالاها را مشاهده کنم"
           )]
    public class GetAllProduct : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly UnitOfWork _unitOfWork;
        private readonly ProductRepository _productRipository;
        private readonly ProductService _sut;
        private Category _category1;
        private Category _category2;
        private List<GetProductDto> _expected;
        public GetAllProduct(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _productRipository = new EFProductRepository(_dataContext);
            _sut = new ProductAppService(_productRipository, _unitOfWork);
        }

        [When("دسته بندی با عنوان 'لبنیات' و 'خشکبار' در فهرست دسته بندی وجود دارد")]
        public void Given()
        {
            _category1 = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(_category1));

            _category2 = CategoryFactory.CreateCategory("خشکبار");
            _dataContext.Manipulate(_ => _.Categories.Add(_category2));
        }

        [And("کالای با کد '1' و '2' در دسته بندی 'لبنیات' و 'خشکبار' وجود دارد")]
        public void And()
        {
            Create_list_of_products();
        }

        [When("درخواست نمایش کالا در دسته بندی 'لبنیات' و 'خشکبار' را میدهیم")]
        public void When()
        {
            
            _expected = _sut.GetAll();
        }

        private void Create_list_of_products()
        {
            var products = new List<Product>
            {
                new Product
                {
                    Code = "1",
                    Name = "شیر کاله",
                    MinimumInventory = 5,
                    Price = 5000,
                    Inventory = 10,
                    CategoryId = _category1.Id
                },

                new Product
                {
                    Code = "2",
                    Name = "test",
                    MinimumInventory = 8,
                    Price = 10000,
                    Inventory = 9,
                    CategoryId = _category2.Id
                },
            };

            _dataContext.Manipulate(_ => _.products.AddRange(products));
        }
    }
}
