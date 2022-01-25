using System;
using System.Collections.Generic;

namespace AIN.FAAS.Repository.Models
{
    public partial class InventoryItem
    {
        public InventoryItem()
        {
            TransferRequest = new HashSet<TransferRequest>();
        }

        public Guid Id { get; set; }
        public string Sgtin { get; set; }
        public Guid? StorageId { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public long? LotNumber { get; set; }
        public Guid? ProductId { get; set; }
        public string Remarks { get; set; }
        public string Status { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual Product Product { get; set; }
        public virtual Storage Storage { get; set; }
        public virtual ICollection<TransferRequest> TransferRequest { get; set; }
    }
}
