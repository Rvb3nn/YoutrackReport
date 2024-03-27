using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutrackReport.DTOs
{
    public class MetricasDTO
    {
        public Project Project { get; set; }
        public string idReadable { get; set; }
        public string summary { get; set; }
        [JsonProperty("customFields")]
        public List<CustomField> customField { get; set; }
        public List<ProjectLinks> links { get; set; }
        [JsonProperty("$type")]
        public string type { get; set; }

    }
    public class CustomField
    {
        public dynamic Value { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
    }
    public class Value
    {
        public dynamic name { get; set; }
        public string Type { get; set; }
    }

    public class Project 
    { 
        public string Name { get; set; }
        public string Type { get; set; }
    }

    // Clase para las vinculaciones
    public class ProjectLinks
    {
        public List<ProjectIssues> issues { get; set; }
        public string ProjectId { get; set; }
    }

    public class ProjectIssues
    {
        public string ProjectId { get; set; }
        public string idReadable { get; set; }
        public string summary { get; set; }
        public string id { get; set; }
    }

}
