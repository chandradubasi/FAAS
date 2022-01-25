using System;
using System.Collections.Generic;

namespace AIN.FAAS.Repository.Models
{
    public partial class Lab
    {
        public Lab()
        {
            Location = new HashSet<Location>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? SiteId { get; set; }

        public virtual Site Site { get; set; }
        public virtual ICollection<Location> Location { get; set; }
    }
}
