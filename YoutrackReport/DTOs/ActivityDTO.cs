
using System.Text.Json;

namespace YoutrackReport.DTOs
{
    public class ActivityDTO
    {
        public string? id { get; set; }
        public ActivityItem target { get; set; }
        public Author author { get; set; }
        public long? timestamp { get; set; }
        public string? type { get; set; }
    }

    public class Author
    {
        public string name { get; set; }
        public string login { get; set; }
        public string type { get; set; }
    }

    public class ActivityItem
    {
        public string id { get; set; }
        public string type { get; set; }
        public string? text { get; set; }
    }
}
