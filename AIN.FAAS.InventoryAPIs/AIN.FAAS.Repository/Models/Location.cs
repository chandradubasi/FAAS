using System;
using System.Collections.Generic;

namespace AIN.FAAS.Repository.Models
{
    public partial class Location
    {
        public Location()
        {
            Storage = new HashSet<Storage>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? LabId { get; set; }

        public virtual Lab Lab { get; set; }
        public virtual ICollection<Storage> Storage { get; set; }
    }
}
