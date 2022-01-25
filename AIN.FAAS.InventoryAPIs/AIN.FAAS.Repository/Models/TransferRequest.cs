using System;
using System.Collections.Generic;

namespace AIN.FAAS.Repository.Models
{
    public partial class TransferRequest
    {
        public Guid Id { get; set; }
        public Guid? Items { get; set; }
        public Guid? SourceId { get; set; }
        public Guid? DestinationId { get; set; }
        public string Requester { get; set; }
        public DateTime? RequestedDate { get; set; }
        public string Approver { get; set; }
        public string Status { get; set; }
        public string Comments { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual Storage Destination { get; set; }
        public virtual InventoryItem ItemsNavigation { get; set; }
        public virtual Storage Source { get; set; }
    }
}
