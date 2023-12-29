using YoutrackReport.DTOs;
using YoutrackReport.Servicios.Contrato;

namespace YoutrackReport.Servicios.Impllementacion
{
    public class ObtieneArchivoAdj : IObtieneArchivoAdj
    {
        private readonly HttpClient _httpClient;

        public ObtieneArchivoAdj(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ArchivoAdjDTO>> ObtieneArchivos()
        {
            var api = "https://prosysspa.youtrack.cloud/api/issues/2-796/attachments?fields=id,name,author(id,name),created,updated,size,mimeType,extension,charset,metaData,url";

            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, api);
                request.Headers.Add("Authorization", "Bearer perm:UGFibG9fRWxndWV0YQ==.NTgtMTA=.LeJvALnkcWG2POgkNBoAoMUb4XyxMR");
                
                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var archivos = await response.Content.ReadFromJsonAsync<List<ArchivoAdjDTO>>();

                foreach (var x in archivos)
                {
                    var tagsJson = x.author.GetRawText();
                }

                return archivos ?? new List<ArchivoAdjDTO>();
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
