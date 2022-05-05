using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Migrations
{
    [Migration(202205051226)]
    public class _202205051226_InitialDataBase : Migration
    {
        public override void Up()
        {
            CreateCategories();

            CreateProducts();

            SalesInvoices();

            PurchaseInvoices();
        }

        public override void Down()
        {
            Delete.Table("Categories");
            Delete.Table("Products");
            Delete.Table("SalesInvoices");
            Delete.Table("PurchaseInvoices");
        }

        private void PurchaseInvoices()
        {
            Create.Table("PurchaseInvoices")
                            .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()
                            .WithColumn("CodeOfProduct").AsString().NotNullable()
                            .WithColumn("NameOfProduct").AsString().NotNullable()
                            .WithColumn("Number").AsInt32().NotNullable()
                            .WithColumn("Price").AsInt32().NotNullable()
                            .WithColumn("Date").AsDate()
                            .WithColumn("ProdutId").AsInt32().NotNullable()
                            .ForeignKey("FK_PurchaseInvoices_Products", "Products", "Id");
        }

        private void SalesInvoices()
        {
            Create.Table("SalesInvoices")
                            .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()
                            .WithColumn("CodeOfProduct").AsString().NotNullable()
                            .WithColumn("NameOfProduct").AsString().NotNullable()
                            .WithColumn("Number").AsInt32().NotNullable()
                            .WithColumn("Date").AsDate()
                            .WithColumn("TotalCost").AsInt32().NotNullable()
                            .WithColumn("ProductId").AsInt32().NotNullable()
                            .ForeignKey("FK_SalesInvoices_Products", "Products", "Id");
        }

        private void CreateProducts()
        {
            Create.Table("Products")
                            .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()
                            .WithColumn("Code").AsString().NotNullable()
                            .WithColumn("Name").AsString().NotNullable()
                            .WithColumn("Price").AsInt32().NotNullable()
                            .WithColumn("Inventory").AsInt32().NotNullable()
                            .WithColumn("MinimumInventory").AsInt32().WithDefaultValue(0)
                            .WithColumn("CategoryId").AsInt32().NotNullable()
                            .ForeignKey("FK_Products_Categories", "Categories", "Id");
        }

        private void CreateCategories()
        {
            Create.Table("Categories")
                            .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()
                            .WithColumn("Name").AsString(50).NotNullable();
        }

       
    }
}
