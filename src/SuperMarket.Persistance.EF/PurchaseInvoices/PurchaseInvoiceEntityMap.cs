using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SuperMarket.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Persistance.EF.PurchaseInvoices
{
    public class PurchaseInvoiceEntityMap : IEntityTypeConfiguration<PurchaseInvoice>
    {
        public void Configure(EntityTypeBuilder<PurchaseInvoice> builder)
        {
            builder.ToTable("PurchaseInvoices");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Property(p => p.CodeOfProduct).IsRequired();

            builder.Property(p => p.NameOfProduct).IsRequired();

            builder.Property(p => p.Number).IsRequired();

            builder.Property(p => p.Price).IsRequired();

            builder.Property(p => p.Date);

            builder.HasOne(p => p.Product)
                .WithMany(p => p.PurchaseInvoices)
                .HasForeignKey(p => p.ProductId);

        }
    }
}
