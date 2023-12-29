using YoutrackReport.DTOs;
using YoutrackReport.Servicios.Contrato;

namespace YoutrackReport.Servicios.Impllementacion
{
    public class ObtieneProyectosServices : IObtieneProyectosServices
    {
        private readonly HttpClient _httpClient;
        public ObtieneProyectosServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<ProyectosDTO>> ObtieneProyectos()
        {
            var api = "https://prosysspa.youtrack.cloud/api/issues?fields=id,summary,description,reporter(login)";

            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, api);
                request.Headers.Add("Authorization", "Bearer perm:Y2d1dGllcnJleg==.NTgtNw==.ZAD9HryU5yjSvPvNr2BvmeMTyUmTMZ");

                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var proyectos = await response.Content.ReadFromJsonAsync<List<ProyectosDTO>>();

                foreach (var p in proyectos)
                {
                    var tagsJson = p.reporter.GetRawText();

                }

                return proyectos ?? new List<ProyectosDTO>();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
