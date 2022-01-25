using System;
using System.Collections.Generic;

namespace AIN.FAAS.Repository.Models
{
    public partial class Site
    {
        public Site()
        {
            Lab = new HashSet<Lab>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? HospitalId { get; set; }

        public virtual Hospital Hospital { get; set; }
        public virtual ICollection<Lab> Lab { get; set; }
    }
}
