
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;
using YoutrackReport.DTOs;
using YoutrackReport.Pages;
using YoutrackReport.Servicios.Contrato;

namespace YoutrackReport.Servicios.Impllementacion
{
    public class ObtieneMetricas
    {

        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        
        public ObtieneMetricas(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
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

            //Envio de la solicitud HTTP y manejo de la respuesta
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            //Lectura del cuerpo de la respuesta y deserializacion JSON
            var body = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<MetricasDTO>>(body);

            return result;
        }



        // Método para obtener y transformar datos comunes
        public async Task<List<FieldsDTO>> ObtenerDatosComunes(string endpoint)
        {
            try
            {
                //Creacion de datatable para almacenar datos tabulares
                DataTable dt = new DataTable();

                //Creacion de columnas basado en propiedades de FieldsDTO
                foreach (var property in typeof(FieldsDTO).GetProperties())
                {
                    // Obtén el nombre de la propiedad
                    string propertyName = property.Name;

                    // Obtén el tipo de datos de la propiedad
                    Type propertyType = property.PropertyType;

                    // Agrega la columna al DataTable
                    dt.Columns.Add(propertyName, propertyType);
                }

                //Obtiene los datos desde la API
                var result = await ObtenerDatosApi(endpoint);
                List<FieldsDTO> datos = new List<FieldsDTO>();

                //Transformacion de datos y llenado del datatable y lista FieldsDTO
                foreach (var res in result)
                {

                    FieldsDTO prueba = new FieldsDTO();


                    if (res.Project != null)
                    {
                        // Asigna directamente el valor a la propiedad Project
                        prueba.Project = res.Project.Name;
                    }

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

                    if (res.customField.FindAll(x => x.Name == "Assignee").FirstOrDefault()?.Value != null)
                    {
                        prueba.Assignee = JsonConvert.DeserializeObject<Value>(res.customField.FindAll(x => x.Name == "Assignee").FirstOrDefault().Value.ToString()).name;
                    }

                    var URLJiraField = res.customField.Find(x => x.Name == "URL Jira");
                    if (URLJiraField?.Value != null)
                    {
                        prueba.URLJira = URLJiraField.Value.ToString();
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
                    prueba.Project,
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
            catch (Exception ex)
            {
                string messoa = ex.Message + "AAAAAAAAAA" + ex.StackTrace;
                throw;
            }
        }


        //Metodo para obtener totales de estados general
        public MetricasKPI CalcularTotales(List<FieldsDTO> metricas)
        {
            MetricasKPI metricasKPI = new MetricasKPI();

            //Llamada al metodo que obtiene la api
            var datos = metricas;

            //Calcular totales            
            metricasKPI.CantidadTerminado = datos.Where(x => x.State == "Terminado" || x.State == "Cerrado").Count();
            metricasKPI.CantidadEnCurso = datos.Count - metricasKPI.CantidadTerminado;

            return metricasKPI;

        }


        //Metodo para cacular totales por jefe de proyecto
        public async Task<MetricasKPI> CalcularTotalesPorJP(List<FieldsDTO> metricas, MetricasKPI metricasKPI)
        {
            // Obtener la lista de jefes de proyecto únicos
            List<string> jefesProyectoUnicos = await ObtenerJefesProyectoUnicos();

            //Obtener datos
            var datos = metricas;
            var datosTer = datos.Where(x => x.State == "Terminado" || x.State == "Cerrado").ToList();

            //Lista para almacenar resultados por jefe de proyecto
            List<KPI_Lista_JP> list_jpKpi = new();

            // Calcular totales por cada jefe de proyecto
            foreach (var jefeProyecto in jefesProyectoUnicos)
            {
                KPI_Lista_JP jpKpi = new();
                jpKpi.NomJP = jefeProyecto;
                jpKpi.CantidadTerminadoJP = datosTer.Where(x => x.JefeDeProyecto == jefeProyecto).Count();
                jpKpi.CantidadDesarrolloJP = datos.Where(x => x.JefeDeProyecto == jefeProyecto && (x.State == "En desarrollo" || x.State == "Pendiente" || x.State == "Detenido" || x.State == "En curso")).Count();
                jpKpi.CantidadDetenidoJP = datos.Where(x => x.JefeDeProyecto == jefeProyecto && (x.State == "Instalación QA" || x.State == "Exitoso completo" || x.State == "Pruebas PROSYS" || x.State == "Fallido" || x.State == "Exitoso liviano")).Count();
                jpKpi.CantidadPorIniciarJP = datos.Where(x => x.JefeDeProyecto == jefeProyecto && (x.State == "Certificación producción" || x.State == "Instalación producción")).Count();

                //Proyectos atrasados por estados de jefe de proyecto
                jpKpi.AtrasoDesarrolloJP = datos.Where(x => x.JefeDeProyecto == jefeProyecto && ((x.FechaTerminoDesa != null ? DateTime.Parse(x.FechaTerminoDesa) : DateTime.Now) < DateTime.Now) && (x.State == "En desarrollo" || x.State == "En curso")).Count();
                jpKpi.AtrasoQAJP = datos.Where(x => x.JefeDeProyecto == jefeProyecto && ((x.FechaTerminoQA != null ? DateTime.Parse(x.FechaTerminoQA) : DateTime.Now) < DateTime.Now) && (x.State == "Instalación QA" || x.State == "Exitoso completo" || x.State == "Pruebas PROSYS" || x.State == "Fallido" || x.State == "Exitoso liviano")).Count();
                jpKpi.AtrasoProduccionJP = datos.Where(x => x.JefeDeProyecto == jefeProyecto && ((x.FechaTerminoReal != null ? DateTime.Parse(x.FechaTerminoReal) : DateTime.Now) < DateTime.Now) && (x.State == "Instalación producción" || x.State == "Certificación producción")).Count();
             

                jpKpi.TotalAtrasosJP = jpKpi.AtrasoDesarrolloJP + jpKpi.AtrasoQAJP + jpKpi.AtrasoProduccionJP;
                jpKpi.TotalJP = jpKpi.CantidadTerminadoJP + jpKpi.CantidadDesarrolloJP + jpKpi.CantidadDetenidoJP + jpKpi.CantidadPorIniciarJP + jpKpi.CantidadEncursoJP;
                jpKpi.CantidadEncursoJP = jpKpi.TotalJP - jpKpi.CantidadTerminadoJP;


                //Cantidad de incidencias con QA y sin QA
                jpKpi.ConQACount = datos
                    .Where(x => x.JefeDeProyecto == jefeProyecto && x.FechaTerminoQA != null &&
                                (x.State == "Instalación QA" || x.State == "Exitoso completo" || x.State == "Pruebas PROSYS" || x.State == "Exitoso liviano"))
                    .Count();

                jpKpi.SinQACount = datos
                    .Where(x => x.JefeDeProyecto == jefeProyecto && x.FechaTerminoQA == null &&
                                (x.State == "Instalación QA" || x.State == "Exitoso completo" || x.State == "Pruebas PROSYS" || x.State == "Exitoso liviano"))
                    .Count();

                jpKpi.QAFallido = datos
                    .Where(x => x.JefeDeProyecto == jefeProyecto && x.FechaTerminoQA == null && x.State == "Fallido")
                    .Count();

                jpKpi.QAPendiente = datos
                    .Where(x => x.JefeDeProyecto == jefeProyecto && x.FechaTerminoQA != null &&
                                (x.State == "Pendiente" || x.State == "En curso"))
                    .Count();

                jpKpi.QAExitoso = datos
                    .Where(x => x.JefeDeProyecto == jefeProyecto && x.FechaTerminoQA != null &&
                                (x.State == "Exitoso completo" || x.State == "Exitoso liviano"))
                    .Count();


                //Añade a la lista
                list_jpKpi.Add(jpKpi);

            }

            //Asignar la lista de resultados a la propiedad de la clase MetricasKPI
            metricasKPI.kPI_Lista_JPs = list_jpKpi;

            return metricasKPI;
        }



        //public List<FieldsDTO> DetallesQAFechasNull (List<FieldsDTO> metricas, string nomJefeProyecto)
        //{
        //    var datos = metricas;

        //    List<FieldsDTO> jpKpi = new();

        //    var jpKpiQASinFecha = datos
        //        .Where(x => x.JefeDeProyecto == nomJefeProyecto && x.FechaTerminoQA == null &&
        //        (x.State == "Instalación QA" || x.State == "Exitoso completo" || x.State == "Pruebas PROSYS" || x.State == "Exitoso liviano"))
        //        .Select(x =>
        //        {
        //            x.Subsystem = x.Subsystem;  // Agregar el campo Subsystem sin perder la información existente
        //            x.Assignee = x.Assignee;
        //            x.IDMh = x.IDMh;
        //            x.URLJira = x.URLJira;
        //            return x;
        //        })
        //        .ToList();


        //    return jpKpiQASinFecha;
        //}




        //Lista de los detalles atrasos por jefe de proyecto
        public List<FieldsDTO> DetallesJpAtrasados (List<FieldsDTO> metricas, string nomJefeProyecto)
        {
            var datos = metricas;
           
            List<FieldsDTO> jpKpi = new();

            var jpKpiDesa = datos
                .Where(x => x.JefeDeProyecto == nomJefeProyecto && ((x.FechaTerminoDesa != null ? DateTime.Parse(x.FechaTerminoDesa) : DateTime.Now) < DateTime.Now) && 
                (x.State == "En desarrollo" || x.State == "En curso"))
                .Select(x =>
                {
                    x.Subsystem = x.Subsystem;  // Agregar el campo Subsystem sin perder la información existente
                    x.Assignee = x.Assignee;
                    x.IDMh = x.IDMh;
                    x.URLJira = x.URLJira;
                    x.Project = x.Project;
                    return x;
                })
                .ToList();

            var jpKpiQA = datos
                .Where(x => x.JefeDeProyecto == nomJefeProyecto && ((x.FechaTerminoQA != null ? DateTime.Parse(x.FechaTerminoQA) : DateTime.Now) < DateTime.Now) && 
                (x.State == "Instalación QA" || x.State == "Exitoso completo" || x.State == "Pruebas PROSYS" || x.State == "Fallido" || x.State == "Exitoso liviano"))
                .Select(x =>
                {
                    x.Subsystem = x.Subsystem;  // Agregar el campo Subsystem sin perder la información existente
                    x.Assignee = x.Assignee;
                    x.IDMh = x.IDMh;
                    x.URLJira = x.URLJira;
                    x.Project = x.Project;
                    return x;
                })
                .ToList();

            var jpKpiProd = datos
            .Where(x => x.JefeDeProyecto == nomJefeProyecto && ((x.FechaTerminoReal != null ? DateTime.Parse(x.FechaTerminoReal) : DateTime.Now) < DateTime.Now) && 
            (x.State == "Instalación producción" || x.State == "Certificación producción"))
            .Select(x =>
            {
                x.Subsystem = x.Subsystem;  // Agregar el campo Subsystem sin perder la información existente
                x.Assignee = x.Assignee;
                x.IDMh = x.IDMh;
                x.URLJira = x.URLJira;
                x.Project = x.Project;
                return x;
            })
            .ToList();

            jpKpi.AddRange(jpKpiDesa);
            jpKpi.AddRange(jpKpiQA);
            jpKpi.AddRange(jpKpiProd);

            return jpKpi;
        }


        //Metodo para tener los jefes de proyectos
        public async Task<List<string>> ObtenerJefesProyectoUnicos()
        {

            var api = "https://prosysspa.youtrack.cloud/api/issues?fields=customFields(name,value(name)),project(name,value)";
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


