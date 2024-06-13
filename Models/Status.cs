using SQLite;

namespace CRM_App.Models
{
    [Table("Statuses")]
    public class Status
    {
        [PrimaryKey, AutoIncrement]
        public int StatusID { get; set; }
        public string StatusName { get; set; }
    }
}
