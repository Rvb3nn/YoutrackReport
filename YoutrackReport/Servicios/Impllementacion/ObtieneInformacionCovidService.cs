using YoutrackReport.DTOs;
using YoutrackReport.Pages;
using YoutrackReport.Servicios.Contrato;

namespace YoutrackReport.Servicios.Impllementacion
{
    public class ObtieneInformacionCovidService : IObtieneInformacionCovidService
    {
        private readonly HttpClient _httpClient;
  
        public ObtieneInformacionCovidService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<DatosHistoricos>> ObtieneInformacionCovid()
        {
            var api = "https://api.covidtracking.com/v1/us/daily.json";

            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<DatosHistoricos>> (api);

                if (response is not null)
                {
                    return response;

                }
                else
                {
                    return response = new List<DatosHistoricos>(); ;
                }
            } 
            catch (Exception ex)
            {

                 throw new Exception(ex.Message);
            }
        }
    }
}
