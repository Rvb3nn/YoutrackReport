using Newtonsoft.Json;
using System.Data;
using YoutrackReport.DTOs;


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



        //public async Task<List<FieldsDTO>> ObtenerDatosComunes()
        //{
        //    try
        //    {
        //        ObtieneMetricasJsonYT obtieneMetricasJsonYT = new();

        //        // Obtiene los datos desde la API
        //        var result = await obtieneMetricasJsonYT.ObtenerDatosApi();
        //        List<FieldsDTO> datos = new List<FieldsDTO>();

        //        foreach (var res in result)
        //        {
        //            FieldsDTO prueba = new FieldsDTO();

        //            var propertiesDTO = typeof(FieldsDTO).GetProperties();

        //            foreach (var propertyDTO in propertiesDTO)
        //            {
        //                string propertyName = propertyDTO.Name;

        //                if (propertyName == "FechaTerminoDesa" || propertyName == "FechaTerminoQA" || propertyName == "FechaTerminoReal")
        //                {
        //                    var fechaProperty = res.customField.Find(x => x.Name == propertyName);

        //                    if (fechaProperty?.Value != null)
        //                    {
        //                        long fechaValue = Convert.ToInt64(fechaProperty.Value);
        //                        DateTime fecha = DateTimeOffset.FromUnixTimeMilliseconds(fechaValue).UtcDateTime;

        //                        propertyDTO.SetValue(prueba, fecha.ToString());
        //                    }
        //                }
        //                else
        //                {
        //                    // Busca si hay un campo en la respuesta que coincida con la propiedad actual
        //                    var field = res.GetType().GetProperty(propertyName);

        //                    if (field != null)
        //                    {
        //                        var value = field.GetValue(res, null);
        //                        propertyDTO.SetValue(prueba, value);
        //                    }
        //                    else
        //                    {
        //                        // Puedes agregar lógica aquí para manejar propiedades adicionales si es necesario
        //                    }
        //                }
        //            }

        //            datos.Add(prueba);
        //        }

        //        return datos;
        //    }
        //    catch (Exception ex)
        //    {
        //        // Manejo de excepciones
        //        throw;
        //    }
        //}





        // Método para obtener y transformar datos comunes
        public async Task<List<FieldsDTO>> ObtenerDatosComunes()
        {
            try
            {
                ////Creacion de datatable para almacenar datos tabulares
                //DataTable dt = new DataTable();

                ////Creacion de columnas basado en propiedades de FieldsDTO
                //foreach (var property in typeof(FieldsDTO).GetProperties())
                //{
                //    // Obtén el nombre de la propiedad
                //    string propertyName = property.Name;

                //    // Obtén el tipo de datos de la propiedad
                //    Type propertyType = property.PropertyType;

                //    // Agrega la columna al DataTable
                //    dt.Columns.Add(propertyName, propertyType);
                //}

                ObtieneMetricasJsonYT obtieneMetricasJsonYT = new();

                //Obtiene los datos desde la API
                var result = await obtieneMetricasJsonYT.ObtenerDatosApi();
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

                    if (res.idReadable != null)
                    {
                        // Asigna directamente el valor a la propiedad idReadable
                        prueba.idReadable = res.idReadable;
                    }

                    if (res.summary != null)
                    {
                        // Asigna directamente el valor a la propiedad summary
                        prueba.summary = res.summary;
                    }

                    //Vinculacion
                    if (res.links != null && res.links.Any())
                    {
                        // Recorrer las vinculaciones
                        foreach (var vinculo in res.links)
                        {
                            // Si la vinculación tiene problemas
                            if (vinculo.issues != null && vinculo.issues.Any())
                            {
                                // Recorrer los problemas
                                foreach (var problema in vinculo.issues)
                                {
                                    // Aquí puedes acceder a los datos del problema
                                    string idProblema = problema.idReadable;
                                    string resumenProblema = problema.summary;
                                }
                            }
                        }
                    }


                    //// Filtra las vinculaciones por proyecto utilizando el idReadable
                    //var vinculacionesProyecto = res.links?
                    //    .Where(link => link.issues?.Any(issue => issue.idReadable?.Equals(prueba.idReadable, StringComparison.OrdinalIgnoreCase) ?? false) ?? false)
                    //    .ToList();

                    //// Agrega a la lista solo si hay vinculaciones con el proyecto correspondiente
                    //if (vinculacionesProyecto != null && vinculacionesProyecto.Any())
                    //{
                    //    datos.Add(prueba);
                    //}





                    if (res.customField.FindAll(x => x.Name == "Type").FirstOrDefault()?.Value != null)
                    {
                        prueba.Type = JsonConvert.DeserializeObject<Value>(res.customField.FindAll(x => x.Name == "Type").FirstOrDefault().Value.ToString()).name;
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

                    //    // Agrega la fila al DataTable
                    //    dt.Rows.Add(new object[] {
                    //    prueba.Project,
                    //    prueba.Subsystem,
                    //    prueba.Type,
                    //    prueba.Priority,
                    //    prueba.State,
                    //    prueba.RechazoHDI,
                    //    prueba.EncargadoHDI,
                    //    prueba.JefeDeProyecto,
                    //    prueba.Assignee,
                    //    prueba.DueDate,
                    //    prueba.Estimacion,
                    //    prueba.FechaInicio,
                    //    prueba.FechaTerminoDesa,
                    //    prueba.FechaTerminoQA,
                    //    prueba.FechaTerminoReal,
                    //    prueba.URLJira,
                    //    prueba.URLBitbucket,
                    //    prueba.URLSonarQube,
                    //    prueba.Dificultad,
                    //    prueba.IDMh,
                    //    prueba.IDAgil,
                    //    prueba.SprintsSeparadosPorComa,
                    //    prueba.Completado,
                    //    prueba.FixedInBuild,
                    //});


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
            List<string> jefesProyectoUnicos = await ObtenerJefesProyectoUnicos(metricas);

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
                jpKpi.CantidadDesarrolloJP = datos.Where(x => x.JefeDeProyecto == jefeProyecto && (x.State == "En desarrollo" || x.State == "En curso")).Count();
                jpKpi.CantidadDetenidoJP = datos.Where(x => x.JefeDeProyecto == jefeProyecto && (x.State == "Detenido")).Count();
                jpKpi.CantidadPorIniciarJP = datos.Where(x => x.JefeDeProyecto == jefeProyecto && (x.State == "Pendiente")).Count();

                //Proyectos atrasados por estados de jefe de proyecto
                jpKpi.AtrasoDesarrolloJP = datos.Where(x => x.JefeDeProyecto == jefeProyecto && ((x.FechaTerminoDesa != null ? DateTime.Parse(x.FechaTerminoDesa) : DateTime.Now) < DateTime.Now) && (x.State == "En desarrollo" || x.State == "En curso")).Count();
                jpKpi.AtrasoQAJP = datos.Where(x => x.JefeDeProyecto == jefeProyecto && ((x.FechaTerminoQA != null ? DateTime.Parse(x.FechaTerminoQA) : DateTime.Now) < DateTime.Now) && (x.State == "Detenido")).Count();
                jpKpi.AtrasoProduccionJP = datos.Where(x => x.JefeDeProyecto == jefeProyecto && ((x.FechaTerminoReal != null ? DateTime.Parse(x.FechaTerminoReal) : DateTime.Now) < DateTime.Now) && (x.State == "Pendiente")).Count();
             

                jpKpi.TotalAtrasosJP = jpKpi.AtrasoDesarrolloJP + jpKpi.AtrasoQAJP + jpKpi.AtrasoProduccionJP;
                jpKpi.TotalJP = jpKpi.CantidadTerminadoJP + jpKpi.CantidadDesarrolloJP + jpKpi.CantidadDetenidoJP + jpKpi.CantidadPorIniciarJP + jpKpi.CantidadEncursoJP;
                jpKpi.CantidadEncursoJP = jpKpi.TotalJP - jpKpi.CantidadTerminadoJP;


                ////Contar incidencias Con QA y rechazos
                //jpKpi.ConQACount = datos
                //    .Count(x => x.JefeDeProyecto == jefeProyecto && !x.TieneQA() && x.Type != "Rechazo");

                //jpKpi.Rechazos = datos
                //    .Count(x => x.JefeDeProyecto == jefeProyecto && !x.TieneQA() && x.Type == "Rechazo");


                // Lista con QA
                var proyectosConQA = datos
                    .Where(x => x.JefeDeProyecto == jefeProyecto && !x.TieneQA() && x.Type != "Rechazo")
                    .ToList();

                // Lista Rechazos
                var proyectosRechazos = datos
                    .Where(x => x.JefeDeProyecto == jefeProyecto && !x.TieneQA() && x.Type == "Rechazo")
                    .ToList();




                jpKpi.QAFallido = datos
                    .Where(x => x.JefeDeProyecto == jefeProyecto && x.State == "Fallido")
                    .Count();

                jpKpi.QAPendiente = datos
                    .Where(x => x.JefeDeProyecto == jefeProyecto && (x.State == "Pendiente" || x.State == "En curso"))
                    .Count();

                jpKpi.QAExitoso = datos
                    .Where(x => x.JefeDeProyecto == jefeProyecto //&& x.FechaTerminoQA != null
                                && (x.State == "Exitoso completo" || x.State == "Exitoso liviano"))
                    .Count();


                //Añade a la lista
                list_jpKpi.Add(jpKpi);

            }

            //Asignar la lista de resultados a la propiedad de la clase MetricasKPI
            metricasKPI.kPI_Lista_JPs = list_jpKpi;

            return metricasKPI;
        }


        ////Metodo para vinculaciones 
        //public async Task<MetricasKPI> LinkVinculadosQA (List<FieldsDTO> metricas, MetricasKPI metricasKPI, string nomJefeProyecto)
        //{
        //    var datos = metricas;



        //}









        //Metodo para los modal de Incidencias con QA y los rechazos
        public List<FieldsDTO> DetallesFiltrosQAModal(List<FieldsDTO> metricas, string nomJefeProyecto, string tipo)
        {
            var datos = metricas;

            List<FieldsDTO> jpKpi = new();

            if (tipo == "Rechazos")
            {

                var proyectosRechazos = datos
                    .Where(x => x.JefeDeProyecto == nomJefeProyecto && !x.TieneQA() && x.Type == "Rechazo")
                .Select(x =>
                {
                    x.Subsystem = x.Subsystem;  // Agregar el campo Subsystem sin perder la información existente
                    x.Assignee = x.Assignee;
                    x.IDMh = x.IDMh;
                    x.URLJira = x.URLJira;
                    x.Type = x.Type;
                    x.idReadable = x.idReadable;
                    x.summary = x.summary;
                    return x;
                })
                .ToList();

                jpKpi.AddRange(proyectosRechazos);
            }
            else if (tipo != "Rechazos")
            {
                var proyectosConQA = datos
                    .Where(x => x.JefeDeProyecto == nomJefeProyecto && !x.TieneQA() && x.Type != "Rechazo")
                    .Select(x =>
                    {
                        x.Subsystem = x.Subsystem;  // Agregar el campo Subsystem sin perder la información existente
                        x.Assignee = x.Assignee;
                        x.IDMh = x.IDMh;
                        x.URLJira = x.URLJira;
                        x.Type = x.Type;
                        x.idReadable = x.idReadable;
                        x.summary = x.summary;
                        return x;
                    })
                .ToList();

                jpKpi.AddRange(proyectosConQA);
            }


            return jpKpi;
        }


        //Metodo para los modal de Atrasados y En curso
        public List<FieldsDTO> DetallesJPModal(List<FieldsDTO> metricas, string nomJefeProyecto, string tipo)
        {
            var datos = metricas;
            List<FieldsDTO> jpKpi = new();

            if (tipo == "Atrasados")
            {
                var jpKpiDesa = datos
                .Where(x => x.JefeDeProyecto == nomJefeProyecto && ((x.FechaTerminoDesa != null ? DateTime.Parse(x.FechaTerminoDesa) : DateTime.Now) < DateTime.Now) &&
                (x.State == "En desarrollo" || x.State == "En curso"))
                .Select(x =>
                {
                    x.Subsystem = x.Subsystem;
                    x.Assignee = x.Assignee;
                    x.IDMh = x.IDMh;
                    x.URLJira = x.URLJira;
                    x.Project = x.Project;
                    x.idReadable = x.idReadable;
                    x.UrlYT = _configuration["UrlIndicencias"] + x.idReadable + "/" + Uri.EscapeDataString(x.summary);
                    return x;
                })
                .ToList();

                var jpKpiQA = datos
                    .Where(x => x.JefeDeProyecto == nomJefeProyecto && ((x.FechaTerminoQA != null ? DateTime.Parse(x.FechaTerminoQA) : DateTime.Now) < DateTime.Now) &&
                    (x.State == "Detenido"))
                    .Select(x =>
                    {
                        x.Subsystem = x.Subsystem;
                        x.Assignee = x.Assignee;
                        x.IDMh = x.IDMh;
                        x.URLJira = x.URLJira;
                        x.Project = x.Project;
                        x.idReadable = x.idReadable;
                        x.UrlYT = _configuration["UrlIndicencias"] + x.idReadable + "/" + Uri.EscapeDataString(x.summary);
                        return x;
                    })
                    .ToList();

                var jpKpiProd = datos
                .Where(x => x.JefeDeProyecto == nomJefeProyecto && ((x.FechaTerminoReal != null ? DateTime.Parse(x.FechaTerminoReal) : DateTime.Now) < DateTime.Now) &&
                (x.State == "Pendiente"))
                .Select(x =>
                {
                    x.Subsystem = x.Subsystem;
                    x.Assignee = x.Assignee;
                    x.IDMh = x.IDMh;
                    x.URLJira = x.URLJira;
                    x.Project = x.Project;
                    x.idReadable = x.idReadable;
                    x.UrlYT = _configuration["UrlIndicencias"] + x.idReadable + "/" + Uri.EscapeDataString(x.summary);
                    return x;
                })
                .ToList();

                jpKpi.AddRange(jpKpiDesa);
                jpKpi.AddRange(jpKpiQA);
                jpKpi.AddRange(jpKpiProd);
            }
            else if (tipo == "En curso")
            {
                var jpKpiEnCurso = datos
                .Where(x => x.JefeDeProyecto == nomJefeProyecto && (x.State == "En desarrollo" || x.State == "Pendiente" || x.State == "Detenido" || x.State == "En curso"))
                .Select(x =>
                {
                    x.Subsystem = x.Subsystem;
                    x.Assignee = x.Assignee;
                    x.IDMh = x.IDMh;
                    x.URLJira = x.URLJira;
                    x.Project = x.Project;
                    x.idReadable = x.idReadable;
                    x.UrlYT = _configuration["UrlIndicencias"] + x.idReadable + "/" + Uri.EscapeDataString(x.summary);
                    return x;
                })
                .ToList();

                jpKpi.AddRange(jpKpiEnCurso);
            }

            return jpKpi;
        }



        //Metodo para tener los jefes de proyectos
        public async Task<List<string>> ObtenerJefesProyectoUnicos(List<FieldsDTO> metricas)
        {

            // Lista para almacenar jefes de proyecto únicos
            List<string> jefesProyectoUnicos = new List<string>();

            foreach (FieldsDTO res in metricas)
            {
                if (res.JefeDeProyecto != null)
                {
                    string jefeProyecto = res.JefeDeProyecto;

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


