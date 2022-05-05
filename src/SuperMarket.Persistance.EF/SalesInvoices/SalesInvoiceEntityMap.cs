using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SuperMarket.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Persistance.EF.SalesInvoices
{
    public class SalesInvoiceEntityMap : IEntityTypeConfiguration<SalesInvoice>
    {
        public void Configure(EntityTypeBuilder<SalesInvoice> builder)
        {
            builder.ToTable("SalesInvoices");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Property(p => p.CodeOfProduct).IsRequired();

            builder.Property(p => p.NameOfProduct).IsRequired();

            builder.Property(p => p.Number).IsRequired();

            builder.Property(p => p.Date);

            builder.Property(p => p.TotalCost);

            builder.HasOne(p => p.Product)
                .WithMany(p => p.SalesInvoices)
                .HasForeignKey(p => p.ProductId);
        }
    }
}
