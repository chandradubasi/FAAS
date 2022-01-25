using System;
using System.Collections.Generic;
using System.Text;

namespace AIN.FAAS.ViewModel.Models.Response
{
    public class LabsResponse
    {
        public string LabId { get; set; }
        public string Name { get; set; }
        public string[] Locations { get; set; }
    }
}
