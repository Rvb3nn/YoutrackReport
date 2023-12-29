
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using YoutrackReport.DTOs;
using YoutrackReport.Servicios.Contrato;

namespace YoutrackReport.Servicios.Impllementacion
{
    public class ObtieneMetricas : IObtieneMetricas
    {
        private readonly HttpClient _httpClient;

        public ObtieneMetricas(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<MetricasDTO>> ObtieneMetricasV()
        {
            //var api = "https://prosysspa.youtrack.cloud/api/issues?fields=summary,customFields(id,name,value(id,name))";
            var api = "https://prosysspa.youtrack.cloud/api/issues?fields=customFields(name,value(name))";

            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, api);
                request.Headers.Add("Authorization", "Bearer perm:UGFibG9fRWxndWV0YQ==.NTgtMTA=.LeJvALnkcWG2POgkNBoAoMUb4XyxMR");

                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var metricasc = await response.Content.ReadFromJsonAsync<List<MetricasDTO>>();

                return metricasc ?? new List<MetricasDTO>();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
