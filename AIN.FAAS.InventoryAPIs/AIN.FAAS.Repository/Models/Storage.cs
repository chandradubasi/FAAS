using System;
using System.Collections.Generic;

namespace AIN.FAAS.Repository.Models
{
    public partial class Storage
    {
        public Storage()
        {
            InventoryItem = new HashSet<InventoryItem>();
            TransferRequestDestination = new HashSet<TransferRequest>();
            TransferRequestSource = new HashSet<TransferRequest>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? LocationId { get; set; }

        public virtual Location Location { get; set; }
        public virtual ICollection<InventoryItem> InventoryItem { get; set; }
        public virtual ICollection<TransferRequest> TransferRequestDestination { get; set; }
        public virtual ICollection<TransferRequest> TransferRequestSource { get; set; }
    }
}
