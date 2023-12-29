//using System.Text.Json.Serialization;
//using YoutrackReport.DTOs;
//using YoutrackReport.Servicios.Contrato;
//using Newtonsoft.Json;

//namespace YoutrackReport.Servicios.Impllementacion
//{
//    public class ObtienePanelPrincipal : IObtienePanelDPrincipal
//    {
//        private readonly HttpClient _httpClient;

//        public ObtienePanelPrincipal(HttpClient httpClient)
//        {
//            _httpClient = httpClient;
//        }
//        public async Task<Root> ObtienePanel(string idProyecto, string idIssue)
//        {
//            var api = $"https://prosysspa.youtrack.cloud/api/admin/projects/{idProyecto}/issues/{idIssue}?fields=id,idReadable,summary,description,customFields(id,name,value(id,name))";

//            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer perm:UGFibG9fRWxndWV0YQ==.NTgtMTA=.LeJvALnkcWG2POgkNBoAoMUb4XyxMR");

//            var response2 = await _httpClient.GetAsync(api);

//            using (var responseStream = await response2.Content.ReadAsStreamAsync())
//            {

//                var data = await System.Text.Json.JsonSerializer.DeserializeAsync<Root>(responseStream);
//                return data;
//            }

//        }
//    }
//}
