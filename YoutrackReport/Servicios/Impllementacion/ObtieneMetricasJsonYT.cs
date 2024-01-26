using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http;
using YoutrackReport.DTOs;

namespace YoutrackReport.Servicios.Impllementacion
{
    public class ObtieneMetricasJsonYT
    {
        private readonly IConfiguration _configuration;

        public ObtieneMetricasJsonYT()
        {
            _configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", false, true)
            .AddJsonFile("appsettings.Development.json", true, true)
            .AddEnvironmentVariables()
            .Build();
        }
        public async Task<List<MetricasDTO>> ObtenerDatosApi()
        {
            string url = $"{_configuration["ApiConfig:Url"]}";
            string bearer = $"{_configuration["ApiConfig:Authorization"]}";

            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url),
                Headers =
                {
                    { "User-Agent", "insomnia/2023.5.8" },
                    { "Authorization", bearer },
                },
            };

            //Envio de la solicitud HTTP y manejo de la respuesta
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            //Lectura del cuerpo de la respuesta y deserializacion JSON
            var body = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<MetricasDTO>>(body);

            return result;
        }
    }
}
