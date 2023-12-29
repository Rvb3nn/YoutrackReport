using System.Text.Json;

namespace YoutrackReport.DTOs
{
    public class ArchivoAdjDTO
    {
        public string? name { get; set; }
        public int? size { get; set; }
        public string? extension { get; set; }
        public long? updated { get; set; }
        public JsonElement author { get; set; }
        public string? metaData { get; set; }
        public long? created { get; set; }
        public string? charset { get; set; }
        public string? url { get; set; }
        public string? mimeType { get; set; }
        public string? id { get; set; }
        public string? type { get; set; }
    }
}
