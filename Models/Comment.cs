using SQLite;

namespace CRM_App.Models
{
    [Table("Comments")]
    public class Comment
    {
        [PrimaryKey, AutoIncrement]
        public int CommentID { get; set; }
        public int RequestID { get; set; }
        public int UserID { get; set; }
        public string CommentText { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
