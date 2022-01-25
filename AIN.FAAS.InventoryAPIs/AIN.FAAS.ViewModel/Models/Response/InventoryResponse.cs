using System;
using System.Collections.Generic;
using System.Text;

namespace AIN.FAAS.ViewModel.Models.Response
{
    public class InventoryResponse
    {
        public string InventoryId { get; set; }
        public string SGTIN { get; set; }
        public string ProductId { get; set; }
        public string LotNumber { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string StorageId { get; set; }
    }
}
