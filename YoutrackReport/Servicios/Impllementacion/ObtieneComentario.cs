using YoutrackReport.DTOs;
using YoutrackReport.Pages;
using YoutrackReport.Servicios.Contrato;

namespace YoutrackReport.Servicios.Impllementacion
{
    public class ObtieneComentario : IObtieneComentario
    {
        private readonly HttpClient _httpClient;

        public ObtieneComentario(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ComentarioDTO>> ObtieneComentarios()
        {
            var api = "https://prosysspa.youtrack.cloud/api/issues/2-796/comments?fields=id,author(login,name,id),deleted,text,updated,visibility(permittedGroups(name,id),permittedUsers(id,name,login))";

            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, api);
                request.Headers.Add("Authorization", "Bearer perm:Y2d1dGllcnJleg==.NTgtNw==.ZAD9HryU5yjSvPvNr2BvmeMTyUmTMZ");
                
                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var comentarios = await response.Content.ReadFromJsonAsync<List<ComentarioDTO>>();

                foreach (var f in comentarios)
                {
                    var tagsJson = f.author.GetRawText();
                }

                return comentarios ?? new List<ComentarioDTO>();
            }
            catch (Exception) 
            {
                throw;
            }  
        }
    }
}
