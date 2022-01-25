using System;
using System.Collections.Generic;

namespace AIN.FAAS.Repository.Models
{
    public partial class Product
    {
        public Product()
        {
            InventoryItem = new HashSet<InventoryItem>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Gtin { get; set; }
        public string CatalogNumber { get; set; }
        public Guid? VendorId { get; set; }

        public virtual Vendor Vendor { get; set; }
        public virtual ICollection<InventoryItem> InventoryItem { get; set; }
    }
}
