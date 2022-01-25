using System;
using System.Collections.Generic;
using System.Text;

namespace AIN.FAAS.ViewModel.Models.Request
{
    public class CheckInRequest
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
        public string StorageId { get; set; }
        public string LotNumber { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string[] SGTIN { get; set; }
        public bool AutoId { get; set; }
        public int Quantity { get; set; }
        public string Remarks { get; set; }      
    }
}
