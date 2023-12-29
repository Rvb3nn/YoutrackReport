using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using YoutrackReport.DTOs; // Asegúrate de tener los espacios de nombres correctos
using YoutrackReport.Servicios.Contrato;

namespace YoutrackReport.Servicios.Impllementacion
{
    public class ObtieneUsuariosService : IObtieneUsuariosYoutrackService
    {
        private readonly HttpClient _httpClient;

        public ObtieneUsuariosService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<UsuariosDTO>> ObtieneUsuarios()
        {
            var apiEndpoint = "https://prosysspa.youtrack.cloud/api/users?fields=id,login,fullName,email,name,jabberAccount,online,avatarUrl,banned,tags(id,name,untagOnResolve,updateableBy(id,name),visibleFor(name,id),owner(id,login))";

            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, apiEndpoint);
                request.Headers.Add("Authorization", "Bearer perm:UGFibG9fRWxndWV0YQ==.NTgtMTA=.LeJvALnkcWG2POgkNBoAoMUb4XyxMR");

                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var usuarios = await response.Content.ReadFromJsonAsync<List<UsuariosDTO>>();
                
                foreach (var usuario in usuarios)
                {
                    var tagsJson = usuario.tags.GetRawText();
                   
                }

                return usuarios ?? new List<UsuariosDTO>();
            }
            catch (HttpRequestException ex)
            {
                
                throw new Exception("Error", ex);
            }
        }
    }
}