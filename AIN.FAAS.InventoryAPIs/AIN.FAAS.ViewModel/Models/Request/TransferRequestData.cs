using System;
using System.Collections.Generic;
using System.Text;

namespace AIN.FAAS.ViewModel.Models.Request
{
    public class TransferRequestData
    {
        public string SourceId { get; set; }
        public string DestinationId { get; set; }
        public string Requester { get; set; }
        public DateTime RequestedDate { get; set; }
        public string Approver { get; set; }
        public string Status { get; set; }
        public string[] Items { get; set; }
        public string Comments { get; set; }
    }
}
