namespace CRM_App.Models
{
    public class CommentReportLine
    {
        public int CommentID { get; set; }
        public string CommentText { get; set; }
        public string UserName { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
