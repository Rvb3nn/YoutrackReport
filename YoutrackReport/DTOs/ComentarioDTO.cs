using System.Text.Json;

namespace YoutrackReport.DTOs
{
    public class ComentarioDTO
    {
        public string? text { get; set; }
        public JsonElement author { get; set; }
        public bool? deleted { get; set; }
        public string? updated { get; set; }
        public JsonElement visibility { get; set; }
        public string? id { get; set; }
        public string? type { get; set; }
    }
}
