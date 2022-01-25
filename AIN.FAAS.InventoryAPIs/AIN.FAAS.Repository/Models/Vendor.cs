using System;
using System.Collections.Generic;

namespace AIN.FAAS.Repository.Models
{
    public partial class Vendor
    {
        public Vendor()
        {
            Product = new HashSet<Product>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        public virtual ICollection<Product> Product { get; set; }
    }
}
