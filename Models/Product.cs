using SQLite;

namespace CRM_App.Models
{
    [Table("Product")]
    public class Product
    {
        [PrimaryKey, AutoIncrement]
        public int ProductID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
