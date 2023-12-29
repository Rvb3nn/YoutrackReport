using System.Text.Json;

namespace YoutrackReport.DTOs
{
    public class UsuariosDTO
    {
        public string name { get; set; }    
        public string fullName { get; set; }
        public string avatarUrl { get; set; }
        public string login { get; set; }
        public string email { get; set; }
        public JsonElement tags { get; set; }
        public bool online { get; set; }
        public bool banned { get; set; }
        public string id { get; set; }
        public string type { get; set; }
    }

    //public class Tags
    //{
    //    public Owner owner { get; set; }
    //    public bool untagOnResolve { get; set; }
    //    public string? visibleFor { get; set; }
    //    public string? updateableBy { get; set; }
    //    public string? name { get; set; }
    //    public string id { get; set; }
    //    public string type { get; set; }

    //}

    //public class Owner
    //{
    //    public string login { get; set; }
    //    public string id { get; set; }
    //    public string type { get; set; }
    //}
}
