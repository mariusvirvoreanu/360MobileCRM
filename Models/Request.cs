using SQLite;

namespace CRM_App.Models
{
    [Table("Requests")]
    public class Request
    {
        [PrimaryKey, AutoIncrement]
        public int RequestID { get; set; }
        public int CustomerID { get; set; }
        public int UserID { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
    }
}
