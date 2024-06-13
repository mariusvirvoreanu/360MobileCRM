using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_App.Models
{
    public class SaleReportLine
    {
        public int SaleID { get; set; }
        public string CustomerName { get; set; }
        public string ProductName { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
    }
}
