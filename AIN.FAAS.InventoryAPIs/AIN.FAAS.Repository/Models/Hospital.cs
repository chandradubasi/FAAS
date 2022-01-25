using System;
using System.Collections.Generic;

namespace AIN.FAAS.Repository.Models
{
    public partial class Hospital
    {
        public Hospital()
        {
            Site = new HashSet<Site>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Site> Site { get; set; }
    }
}
