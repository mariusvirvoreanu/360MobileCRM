using SQLite;

namespace CRM_App.Models
{
    [Table("Sales")]
    public class Sale
    {
        [PrimaryKey, AutoIncrement]
        public int SaleID { get; set; }
        public int CustomerID { get; set; }
        public int ProductID { get; set; }
        public decimal TotalAmount { get; set; }
        public int UserID { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
    }
}
