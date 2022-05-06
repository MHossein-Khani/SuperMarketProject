using Microsoft.EntityFrameworkCore;
using SuperMarket.Entities;
using SuperMarket.Persistance.EF.Categories;

namespace SuperMarket.Persistance.EF
{
    public class EFDataContext : DbContext
    {
        public EFDataContext(string connectionString) :
           this(new DbContextOptionsBuilder().UseSqlServer(connectionString).Options)
        { }

        public EFDataContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly
                (typeof(CategoryEntityMap).Assembly);
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> products { get; set; }
        public DbSet<SalesInvoice> SalesInvoices { get; set; }
        public DbSet<PurchaseInvoice> PurchaseInvoices { get; set; }

    }
}
