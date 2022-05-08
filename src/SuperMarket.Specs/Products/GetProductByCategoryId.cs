﻿using FluentAssertions;
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
using System.Collections.Generic;
using System.Linq;
using Xunit;
using static SuperMarket.Specs.BDDHelper;

namespace SuperMarket.Specs.Products
{
    public class GetAllProduct : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly UnitOfWork _unitOfWork;
        private readonly ProductRepository _productRipository;
        private readonly ProductService _sut;
        private Category _category;
        private List<Product> _products;

        public GetAllProduct(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _productRipository = new EFProductRepository(_dataContext);
            _sut = new ProductAppService(_productRipository, _unitOfWork);
        }

        [When("دسته بندی با عنوان 'لبنیات' در فهرست دسته بندی وجود دارد")]
        public void Given()
        {
            _category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(_category));
        }

        [And("کالای با کد '1' و '2' در دسته بندی 'لبنیات' وجود دارد")]
        public void And()
        {
            _products = new List<Product>
            {
                new Product
                {
                    Code = "1",
                    Name = "شیر کاله",
                    MinimumInventory = 5,
                    Price = 5000,
                    Inventory = 10,
                    CategoryId = _category.Id
                },

                new Product
                {
                    Code = "2",
                    Name = "test",
                    MinimumInventory = 8,
                    Price = 10000,
                    Inventory = 9,
                    CategoryId = _category.Id
                },
            };
        }

        [When("درخواست نمایش کالا در دسته بندی 'لبنیات' را میدهیم")]
        public void When()
        {
            var dto = new GetProductDto
            {
                CategoryId = _category.Id
            };
            _sut.Get(dto.CategoryId);
        }

        //private void Create_list_of_products()
        //{
        //    _products = new List<Product>
        //    {
        //        new Product
        //        {
        //            Code = "1",
        //            Name = "شیر کاله",
        //            MinimumInventory = 5,
        //            Price = 5000,
        //            Inventory = 10,
        //            CategoryId = _category.Id
        //        },

        //        new Product
        //        {
        //            Code = "2",
        //            Name = "test",
        //            MinimumInventory = 8,
        //            Price = 10000,
        //            Inventory = 9,
        //            CategoryId = _category.Id
        //        },
        //    };
        //}
    }
}
