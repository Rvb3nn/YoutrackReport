
using Newtonsoft.Json;
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

        public async IAsyncEnumerable<string> CalcularTotales()
        {
            List<FieldsDTO> datos = new List<FieldsDTO>();
            int CantidadEnCurso = 0;
            int CantidadTerminado = 0;

            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://prosysspa.youtrack.cloud/api/issues?fields=customFields(name,value(name))"),
                Headers =
            {
                { "User-Agent", "insomnia/2023.5.8" },
                { "Authorization", "Bearer perm:UGFibG9fRWxndWV0YQ==.NTgtMTA=.LeJvALnkcWG2POgkNBoAoMUb4XyxMR" },
            },
            };

            var response = await client.SendAsync(request);
            var body = await response.Content.ReadAsStringAsync();
            var resul = JsonConvert.DeserializeObject<List<MetricasDTO>>(body);


            //recorro "resul", variable que contiene el resultado de la petición al servicio
            //para recorrer la variable "resul", esta es asignada a una nueva variable de recorrido llamada "res"
            foreach (MetricasDTO res in resul)
            {
                FieldsDTO prueba = new FieldsDTO();

                // if (res.customField.FindAll(x => x.Name == "Subsystem").FirstOrDefault().Value != null)
                // {
                //     prueba.Subsystem = JsonConvert.DeserializeObject<Value>(res.customField.FindAll(x => x.Name == "Subsystem").FirstOrDefault().Value.ToString()).name;
                // }

                // if (res.customField.FindAll(x => x.Name == "Type").FirstOrDefault().Value != null)
                // {
                //     prueba.Type = JsonConvert.DeserializeObject<Value>(res.customField.FindAll(x => x.Name == "Type").FirstOrDefault().Value.ToString()).name;
                // }

                // if (res.customField.FindAll(x => x.Name == "Priority").FirstOrDefault().Value != null)
                // {
                //     prueba.Priority = JsonConvert.DeserializeObject<Value>(res.customField.FindAll(x => x.Name == "Priority").FirstOrDefault().Value.ToString()).name;
                // }

                if (res.customField.FindAll(x => x.Name == "State").FirstOrDefault().Value != null)
                {
                    prueba.State = JsonConvert.DeserializeObject<Value>(res.customField.FindAll(x => x.Name == "State").FirstOrDefault().Value.ToString()).name;
                }

                // if (res.customField.FindAll(x => x.Name == "Rechazo HDI").FirstOrDefault().Value != null)
                // {
                //     prueba.RechazoHDI = JsonConvert.DeserializeObject<Value>(res.customField.FindAll(x => x.Name == "Rechazo HDI").FirstOrDefault().Value.ToString()).name;
                // }

                // if (res.customField.FindAll(x => x.Name == "Encargado HDI").FirstOrDefault().Value != null)
                // {
                //     prueba.EncargadoHDI = JsonConvert.DeserializeObject<Value>(res.customField.FindAll(x => x.Name == "Encargado HDI").FirstOrDefault().Value.ToString()).name;
                // }

                // if (res.customField.FindAll(x => x.Name == "Jefe de proyecto").FirstOrDefault().Value != null)
                // {
                //     prueba.JefeDeProyecto = JsonConvert.DeserializeObject<Value>(res.customField.FindAll(x => x.Name == "Jefe de proyecto").FirstOrDefault().Value.ToString()).name;
                // }

                // if (res.customField.FindAll(x => x.Name == "Assignee").FirstOrDefault().Value)
                // {
                //     prueba.Assignee = JsonConvert.DeserializeObject<Value>(res.customField.FindAll(x => x.Name == "Assignee").FirstOrDefault().Value.ToString()).name;
                // }

                // if (res.customField.FindAll(x => x.Name == "Due Date").FirstOrDefault().Value)
                // {
                //     prueba.DueDate = res.customField.FindAll(x => x.Name == "Due Date").FirstOrDefault().Value.ToString();      //No deserializa porque aqui entra directo al valor
                // }

                // if (res.customField.FindAll(x => x.Name == "Estimacion").FirstOrDefault().Value)
                // {
                //     prueba.Estimacion = JsonConvert.DeserializeObject<Value>(res.customField.FindAll(x => x.Name == "Estimacion").FirstOrDefault().Value.ToString()).name;
                // }

                // if (res.customField.FindAll(x => x.Name == "FechaInicio").FirstOrDefault().Value)
                // {
                //     prueba.FechaInicio = JsonConvert.DeserializeObject<Value>(res.customField.FindAll(x => x.Name == "FechaInicio").FirstOrDefault().Value.ToString()).name;
                // }

                // if (res.customField.FindAll(x => x.Name == "FechaTerminoDesa").FirstOrDefault().Value)
                // {
                //     prueba.FechaTerminoDesa = JsonConvert.DeserializeObject<Value>(res.customField.FindAll(x => x.Name == "FechaTerminoDesa").FirstOrDefault().Value.ToString()).name;
                // }

                // if (res.customField.FindAll(x => x.Name == "FechaTerminoQA").FirstOrDefault().Value)
                // {
                //     prueba.FechaTerminoQA = JsonConvert.DeserializeObject<Value>(res.customField.FindAll(x => x.Name == "FechaTerminoQA").FirstOrDefault().Value.ToString()).name;
                // }

                // if (res.customField.FindAll(x => x.Name == "FechaTerminoReal").FirstOrDefault().Value)
                // {
                //     prueba.FechaTerminoReal = JsonConvert.DeserializeObject<Value>(res.customField.FindAll(x => x.Name == "FechaTerminoReal").FirstOrDefault().Value.ToString()).name;
                // }

                // if (res.customField.FindAll(x => x.Name == "URL Jira").FirstOrDefault().Value)
                // {
                //     prueba.URLJira = res.customField.FindAll(x => x.Name == "URL Jira").FirstOrDefault().Value.ToString();          //No deserializa porque aqui entra directo al valor
                // }

                // if (res.customField.FindAll(x => x.Name == "URL Bitbucket").FirstOrDefault().Value)
                // {
                //     prueba.URLBitbucket = res.customField.FindAll(x => x.Name == "URL Bitbucket").FirstOrDefault().Value.ToString();        //No deserializa porque aqui entra directo al valor
                // }

                // if (res.customField.FindAll(x => x.Name == "URLSonarQube").FirstOrDefault().Value)
                // {
                //     prueba.URLSonarQube = JsonConvert.DeserializeObject<Value>(res.customField.FindAll(x => x.Name == "URLSonarQube").FirstOrDefault().Value.ToString()).name;
                // }

                // if (res.customField.FindAll(x => x.Name == "Dificultad").FirstOrDefault().Value)
                // {
                //     prueba.Dificultad = JsonConvert.DeserializeObject<Value>(res.customField.FindAll(x => x.Name == "Dificultad").FirstOrDefault().Value.ToString()).name;
                // }

                // if (res.customField.FindAll(x => x.Name == "ID Mh").FirstOrDefault().Value)
                // {
                //     prueba.IDMh = JsonConvert.DeserializeObject<Value>(res.customField.FindAll(x => x.Name == "ID Mh").FirstOrDefault().Value.ToString()).name;
                // }

                // if (res.customField.FindAll(x => x.Name == "IDAgil").FirstOrDefault().Value)
                // {
                //     prueba.IDAgil = JsonConvert.DeserializeObject<Value>(res.customField.FindAll(x => x.Name == "IDAgil").FirstOrDefault().Value.ToString()).name;
                // }

                // if (res.customField.FindAll(x => x.Name == "SprintsSeparadosPorComa").FirstOrDefault().Value)
                // {
                //     prueba.SprintsSeparadosPorComa = JsonConvert.DeserializeObject<Value>(res.customField.FindAll(x => x.Name == "SprintsSeparadosPorComa").FirstOrDefault().Value.ToString()).name;
                // }

                // if (res.customField.FindAll(x => x.Name == "Completado").FirstOrDefault().Value)
                // {
                //     prueba.Completado = JsonConvert.DeserializeObject<Value>(res.customField.FindAll(x => x.Name == "Completado").FirstOrDefault().Value.ToString()).name;
                // }

                // if (res.customField.FindAll(x => x.Name == "FixedInBuild").FirstOrDefault().Value)
                // {
                //     prueba.FixedInBuild = JsonConvert.DeserializeObject<Value>(res.customField.FindAll(x => x.Name == "FixedInBuild").FirstOrDefault().Value.ToString()).name;
                // }


                //al final de cada recorrido del foreach, lleno la lista "datos" (FieldsDTO) 
                datos.Add(prueba);

            }

            //Calcular totales
            CantidadEnCurso = datos.FindAll(x => x.State == "En curso").ToList().Count();
            CantidadTerminado = datos.FindAll(x => x.State == "Terminado").ToList().Count();

            //creo un objeto "resultado" para poder entregar mas de un valor en el método
            var resultado = new
            {
                enCurso = CantidadEnCurso,
                terminado = CantidadTerminado
            };

            //convierto el objeto a JSON
            string jsonResultado = JsonConvert.SerializeObject(resultado);

            //yield return es para devolver resultados en forma de iteración 
            //en vez de devolver todo de una, va iterando el return y lo puedes recorrer con un foreach
            yield return jsonResultado;

        }
    }
}
