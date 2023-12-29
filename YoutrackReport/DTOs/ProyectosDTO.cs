using System.Text.Json;

namespace YoutrackReport.DTOs
{
    public class ProyectosDTO
    {
        public string? summary { get; set; }
        public JsonElement reporter { get; set; }
        public string? description { get; set; }
        public string? id { get; set; }
        public string? type { get; set; }
    }
}
