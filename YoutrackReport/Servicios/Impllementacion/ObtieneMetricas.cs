
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using YoutrackReport.DTOs;
using YoutrackReport.Pages;
using YoutrackReport.Servicios.Contrato;

namespace YoutrackReport.Servicios.Impllementacion
{
    public class ObtieneMetricas
    {

        private readonly HttpClient _httpClient;

        public ObtieneMetricas(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        //METODO QUE CONSUME API
        private async Task<List<MetricasDTO>> ObtenerDatosApi(string endpoint)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(endpoint),
                Headers =
                {
                    { "User-Agent", "insomnia/2023.5.8" },
                    { "Authorization", "Bearer perm:TWFyaW9fUmFtaXJleg==.NTgtMTU=.3iMaKjmBBMj6eXomhtsZ0eBvNAuyNB" },
                },
            };

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var body = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<MetricasDTO>>(body);

            return result;
        }



        // Método para obtener y transformar datos comunes
        public async Task<List<FieldsDTO>> ObtenerDatosComunes(string endpoint)
        {
            try
            {
                DataTable dt = new DataTable();

                foreach (var property in typeof(FieldsDTO).GetProperties())
                {
                    // Obtén el nombre de la propiedad
                    string propertyName = property.Name;

                    // Obtén el tipo de datos de la propiedad
                    Type propertyType = property.PropertyType;

                    // Agrega la columna al DataTable
                    dt.Columns.Add(propertyName, propertyType);
                }

                var result = await ObtenerDatosApi(endpoint);
                List<FieldsDTO> datos = new List<FieldsDTO>();


                foreach (var res in result)
                {


                    FieldsDTO prueba = new FieldsDTO();

                    if (res.customField.FindAll(x => x.Name == "Subsystem").FirstOrDefault()?.Value != null)
                    {
                        prueba.Subsystem = JsonConvert.DeserializeObject<Value>(res.customField.FindAll(x => x.Name == "Subsystem").FirstOrDefault().Value.ToString()).name;
                    }

                    if (res.customField.FindAll(x => x.Name == "State").FirstOrDefault()?.Value != null)
                    {
                        prueba.State = JsonConvert.DeserializeObject<Value>(res.customField.FindAll(x => x.Name == "State").FirstOrDefault().Value.ToString()).name;
                    }

                    if (res.customField.FindAll(x => x.Name == "Jefe de proyecto").FirstOrDefault()?.Value != null)
                    {
                        prueba.JefeDeProyecto = JsonConvert.DeserializeObject<Value>(res.customField.FindAll(x => x.Name == "Jefe de proyecto").FirstOrDefault().Value.ToString()).name;
                    }

                    //FECHAS
                    //Valida el campo Fecha termino desa
                    var cantidad = res.customField.FindAll(x => x.Name == "Fecha Termino Desa").Count();
                    if (cantidad > 0)
                    {
                        if (res.customField.FindAll(x => x.Name == "Fecha Termino Desa").FirstOrDefault().Value != null)
                        {
                            long fechaTerminoDesaValue = Convert.ToInt64(res.customField.FindAll(x => x.Name == "Fecha Termino Desa").FirstOrDefault().Value);
                            DateTime fechaTerminoDesa = DateTimeOffset.FromUnixTimeMilliseconds(fechaTerminoDesaValue).UtcDateTime;

                            prueba.FechaTerminoDesa = fechaTerminoDesa.ToString();
                        }
                    }

                    if (res.customField.FindAll(x => x.Name == "Fecha termino real").FirstOrDefault().Value != null)
                    {
                        long fechaTerminoRealValue = Convert.ToInt64(res.customField.FindAll(x => x.Name == "Fecha termino real").FirstOrDefault().Value);
                        DateTime fechaTerminoReal = DateTimeOffset.FromUnixTimeMilliseconds(fechaTerminoRealValue).UtcDateTime;

                        prueba.FechaTerminoReal = fechaTerminoReal.ToString();
                    }

                    var cantidadQA = res.customField.FindAll(x => x.Name == "Fecha Termino QA").Count();
                    if (cantidadQA > 0)
                    {
                        if (res.customField.FindAll(x => x.Name == "Fecha Termino QA").FirstOrDefault().Value != null)
                        {
                            long fechaTerminoQAValue = Convert.ToInt64(res.customField.FindAll(x => x.Name == "Fecha Termino QA").FirstOrDefault().Value);
                            DateTime fechaTerminoQA = DateTimeOffset.FromUnixTimeMilliseconds(fechaTerminoQAValue).UtcDateTime;

                            prueba.FechaTerminoQA = fechaTerminoQA.ToString();
                        }
                    }


                    var idMhField = res.customField.Find(x => x.Name == "ID MH");
                    if (idMhField?.Value != null)
                    {
                        prueba.IDMh = idMhField.Value.ToString();
                    }

                    // Agrega la fila al DataTable
                    dt.Rows.Add(new object[] {
                    prueba.Subsystem,
                    prueba.Type,
                    prueba.Priority,
                    prueba.State,
                    prueba.RechazoHDI,
                    prueba.EncargadoHDI,
                    prueba.JefeDeProyecto,
                    prueba.Assignee,
                    prueba.DueDate,
                    prueba.Estimacion,
                    prueba.FechaInicio,
                    prueba.FechaTerminoDesa,
                    prueba.FechaTerminoQA,
                    prueba.FechaTerminoReal,
                    prueba.URLJira,
                    prueba.URLBitbucket,
                    prueba.URLSonarQube,
                    prueba.Dificultad,
                    prueba.IDMh,
                    prueba.IDAgil,
                    prueba.SprintsSeparadosPorComa,
                    prueba.Completado,
                    prueba.FixedInBuild,
                });


                    //dt.Rows.Add(prueba.ToArray());


                    datos.Add(prueba);
                }

                return datos;

            }
            catch (Exception)
            {

                throw;
            }
        }


        //Metodo para obtener totales de estados general
        public async IAsyncEnumerable<MetricasKPI> CalcularTotales(List<FieldsDTO> metricas)
        {
            MetricasKPI metricasKPI = new MetricasKPI();

            //Llamada al metodo que obtiene la api
            var datos = metricas;

            //Calcular totales
            metricasKPI.CantidadEnCurso = datos.FindAll(x => x.State == "En curso").ToList().Count();
            metricasKPI.CantidadTerminado = datos.FindAll(x => x.State == "Terminado").ToList().Count();

            return metricasKPI;

        }


        //Metodo para cacular totales por jefe de proyecto
        public async IAsyncEnumerable<string> CalcularTotalesPorJP(List<FieldsDTO> metricas)
        {
            // Obtener la lista de jefes de proyecto únicos
            List<string> jefesProyectoUnicos = await ObtenerJefesProyectoUnicos();

            //Llamada al metodo que obtiene la api

            var datos = metricas;

            var datosTer = datos.Where(x => x.State == "Terminado" || x.State == "Cerrado").ToList();
           

            // Calcular totales por cada jefe de proyecto
            foreach (var jefeProyecto in jefesProyectoUnicos)
            {

                int CantidadTerminadoJP = datosTer.Where(x => x.JefeDeProyecto == jefeProyecto).Count();
                int CantidadDesarrolloJP = datos.Where(x => x.State == "En desarrollo" || x.State == "Pendiente" || x.State == "Detenido" || x.State == "En curso" && x.JefeDeProyecto == jefeProyecto).Count();
                int CantidadDetenidoJP = datos.FindAll(x => x.State == "Instalación QA" || x.State == "Exitoso completo" || x.State == "Pruebas PROSYS" || x.State == "Fallido" || x.State == "Exitoso liviano" && x.JefeDeProyecto == jefeProyecto).Count();
                int CantidadPorIniciarJP = datos.FindAll(x => x.State == "Certificación producción" || x.State == "Instalación producción" && x.JefeDeProyecto == jefeProyecto).Count();


                //Probar atrasos JP
                //List<FieldsDTO> lista = datos.Where(x => x.JefeDeProyecto == "Javier Zapata" && (x.FechaTerminoQA != null ? DateTime.Parse(x.FechaTerminoQA) : DateTime.Now) < DateTime.Now && x.State == "Instalación QA").ToList();
                //List<FieldsDTO> listaDe = datos.Where(x => x.JefeDeProyecto == "Barbara Jeria" &&((x.FechaTerminoDesa != null ? DateTime.Parse(x.FechaTerminoDesa) : DateTime.Now) < DateTime.Now) &&(x.State == "En desarrollo" || x.State == "Pendiente" || x.State == "Detenido" || x.State == "En curso")).ToList();
                //List<FieldsDTO> listaQA = datos.Where(x => x.JefeDeProyecto == "Javier Zapata" && ((x.FechaTerminoQA != null ? DateTime.Parse(x.FechaTerminoQA) : DateTime.Now) < DateTime.Now) && (x.State == "Instalación QA" || x.State == "Exitoso completo" || x.State == "Pruebas PROSYS" || x.State == "Fallido" || x.State == "Exitoso liviano")).ToList();

                int CantidadAtrasosDesa = datos.Where(x => x.JefeDeProyecto == jefeProyecto &&((x.FechaTerminoDesa != null ? DateTime.Parse(x.FechaTerminoDesa) : DateTime.Now) < DateTime.Now) &&(x.State == "En desarrollo" || x.State == "Pendiente" || x.State == "Detenido" || x.State == "En curso")).Count();
                int CantidadAtrasosQA = datos.Where(x => x.JefeDeProyecto == jefeProyecto &&((x.FechaTerminoQA != null ? DateTime.Parse(x.FechaTerminoQA) : DateTime.Now) < DateTime.Now) &&(x.State == "Instalación QA" || x.State == "Exitoso completo" || x.State == "Pruebas PROSYS" || x.State == "Fallido" || x.State == "Exitoso liviano")).Count();
                //int CantidadAtrasosQA = datos.Where(x => x.JefeDeProyecto == jefeProyecto && (x.FechaTerminoQA != null ? DateTime.Parse(x.FechaTerminoQA) : DateTime.Now) < DateTime.Now && x.State == "Instalación QA").Count();
                int CantidadAtrasosReal = datos.Where(x => x.JefeDeProyecto == jefeProyecto && (DateTime.Parse(x.FechaTerminoReal) < DateTime.Now && x.State != "Terminado")).Count();


                int totalProyectos = CantidadTerminadoJP + CantidadDesarrolloJP + CantidadDetenidoJP + CantidadPorIniciarJP;
                //int totalAtrasos = CantidadAtrasosDesa + CantidadAtrasosReal + CantidadAtrasosQA;

                int CantidadEnCursoJP = totalProyectos - CantidadTerminadoJP;


                // Crear un objeto con los totales por cada jefe de proyecto
                var resultadoJP = new
                {
                    jefeProyecto,
                    enCursoJP = CantidadEnCursoJP,
                    terminadoJP = CantidadTerminadoJP,
                    desarrolloJP = CantidadDesarrolloJP,
                    detenidoJP = CantidadDetenidoJP,
                    porIniciarJP = CantidadPorIniciarJP,
                    //atrasosDesa = CantidadAtrasosDesa,
                    atrasosReal = CantidadAtrasosReal,
                    //atrasosQA = CantidadAtrasosQA,
                    totalProyectos,
                    //totalAtrasos
                };

                // Convertir el objeto a JSON y devolverlo
                string jsonResultadoJP = JsonConvert.SerializeObject(resultadoJP);
                yield return jsonResultadoJP;
            }
        }


        //Metodo para tener los jefes de proyectos
        public async Task<List<string>> ObtenerJefesProyectoUnicos()
        {

            var api = "https://prosysspa.youtrack.cloud/api/issues?fields=customFields(name,value(name))";
            var result = await ObtenerDatosApi(api);

            // Lista para almacenar jefes de proyecto únicos
            List<string> jefesProyectoUnicos = new List<string>();

            foreach (MetricasDTO res in result)
            {
                if (res.customField.FindAll(x => x.Name == "Jefe de proyecto").FirstOrDefault()?.Value != null)
                {
                    string jefeProyecto = JsonConvert.DeserializeObject<Value>(res.customField.FindAll(x => x.Name == "Jefe de proyecto").FirstOrDefault().Value.ToString()).name;

                    // Agrega a la lista si aún no está presente
                    if (!jefesProyectoUnicos.Contains(jefeProyecto))
                    {
                        jefesProyectoUnicos.Add(jefeProyecto);
                    }
                }
            }

            return jefesProyectoUnicos;
        }

    }
}


