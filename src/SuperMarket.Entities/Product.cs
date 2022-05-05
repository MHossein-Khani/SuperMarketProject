using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Entities
{
    public class Product
    {

        public Product()
        {
            SalesInvoices = new List<SalesInvoice>();
            EntryDocuments = new List<EntryDocument>();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Inventory { get; set; }
        public int MinimumInventory { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public List<SalesInvoice> SalesInvoices { get; set; }

        public List<EntryDocument> EntryDocuments { get; set; }
    }
}
