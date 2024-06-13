using SQLite;

namespace CRM_App.Models
{
    [Table("Customers")]
    public class Customer
    {
        [PrimaryKey, AutoIncrement]
        public int CustomerID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }
}
