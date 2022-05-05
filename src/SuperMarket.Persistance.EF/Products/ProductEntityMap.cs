using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SuperMarket.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Persistance.EF.Products
{
    public class ProductEntityMap : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");
            
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Property(p => p.Name).IsRequired();

            builder.Property(p => p.Code).IsRequired();

            builder.Property(p => p.Price).IsRequired();

            builder.Property(p => p.Inventory).IsRequired();

            builder.Property(p => p.MinimumInventory).IsRequired();

            builder.HasOne(p => p.Category)
                .WithMany(p => p.Products)
                .HasForeignKey(p => p.CategoryId);


        }
    }
}
