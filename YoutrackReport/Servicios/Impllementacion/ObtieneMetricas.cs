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
                            // Asignar el identificador del proyecto a la vinculación
                            vinculo.ProjectId = res.idReadable;

                            // Si la vinculación tiene problemas
                            if (vinculo.issues != null && vinculo.issues.Any())
                            {
                                // Recorrer los problemas
                                foreach (var problema in vinculo.issues)
                                {
                                    // Asignar el identificador del proyecto al issue
                                    problema.ProjectId = res.idReadable;

                                    // Aquí puedes acceder a los datos del problema
                                    string idProblema = problema.idReadable;
                                    string resumenProblema = problema.summary;
                                }
                            }
                        }
                    }




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
            metricasKPI.CantidadTerminado2023 = datos
                .Where(x => (x.State == "Terminado" || x.State == "Cerrado")
                            && x.FechaTerminoReal != null
                            && DateTime.Parse(x.FechaTerminoReal).Year == 2023)     //No toma las fechas nulas
                .Count();

            metricasKPI.CantidadTerminado2024 = datos
                .Where(x => (x.State == "Terminado" || x.State == "Cerrado")
                            && x.FechaTerminoReal != null
                            && DateTime.Parse(x.FechaTerminoReal).Year == 2024)     //No toma las fechas nulas
                .Count();

            //// Imprimir contenido de proyectosConQA
            //Console.WriteLine("Resultados 2024: ");
            //foreach (var item in CantidadTerminado2024)
            //{
            //    Console.WriteLine($"IdReadable: {item.idReadable}, FechaTerminoReal: {item.FechaTerminoReal}");
            //}

            // Llamada al método para obtener TotalAtrasosJP
            //metricasKPI.TotalAtrasosJP = await CalcularTotalesPorJP(metricas, metricasKPI);


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

            int totalRechazos = 0;

            // Calcular totales por cada jefe de proyecto
            foreach (var jefeProyecto in jefesProyectoUnicos)
            {
                KPI_Lista_JP jpKpi = new();
                jpKpi.NomJP = jefeProyecto;
                jpKpi.CantidadTerminadoJP = datosTer.Where(x => x.JefeDeProyecto == jefeProyecto).Count();
                jpKpi.CantidadDesarrolloJP = datos.Where(x => x.JefeDeProyecto == jefeProyecto && (x.State == "En desarrollo" || x.State == "En curso" || x.State == "Pendiente" || x.State == "Detenido")).Count();
                jpKpi.CantidadQAJP = datos.Where(x => x.JefeDeProyecto == jefeProyecto && (x.State == "Instalación QA" || x.State == "Exitoso completo" || x.State == "Fallido" || x.State == "Exitoso liviano" || x.State == "Pruebas" || x.State == "Pruebas PROSYS")).Count();
                jpKpi.CantidadProdJP = datos.Where(x => x.JefeDeProyecto == jefeProyecto && (x.State == "Instalación producción" || x.State == "Certificación producción")).Count();

                //Proyectos atrasados por fecha 
                jpKpi.AtrasoDesarrolloJP = datos
                    .Where(x => x.JefeDeProyecto == jefeProyecto &&
                                ((x.FechaTerminoDesa != null ? DateTime.Parse(x.FechaTerminoDesa).Date : DateTime.Now.Date) < DateTime.Now.Date) &&
                                (x.State == "En desarrollo" || x.State == "En curso" || x.State == "Pendiente"))
                    .Count();

                jpKpi.AtrasoQAJP = datos
                    .Where(x => x.JefeDeProyecto == jefeProyecto &&
                                ((x.FechaTerminoQA != null ? DateTime.Parse(x.FechaTerminoQA).Date : DateTime.Now.Date) < DateTime.Now.Date) &&
                                (x.State == "Instalación QA" || x.State == "Exitoso completo" || x.State == "Fallido" || x.State == "Exitoso liviano" || x.State == "Pruebas" || x.State == "Pruebas PROSYS"))
                    .Count();

                jpKpi.AtrasoProduccionJP = datos
                    .Where(x => x.JefeDeProyecto == jefeProyecto &&
                                ((x.FechaTerminoReal != null ? DateTime.Parse(x.FechaTerminoReal).Date : DateTime.Now.Date) < DateTime.Now.Date) &&
                                (x.State == "Instalación producción" || x.State == "Certificación producción"))
                    .Count();

                jpKpi.TotalAtrasosJP = jpKpi.AtrasoDesarrolloJP + jpKpi.AtrasoQAJP + jpKpi.AtrasoProduccionJP;
                jpKpi.TotalJP = jpKpi.CantidadTerminadoJP + jpKpi.CantidadDesarrolloJP + jpKpi.CantidadQAJP + jpKpi.CantidadProdJP + jpKpi.CantidadEncursoJP;
                jpKpi.CantidadEncursoJP = jpKpi.TotalJP - jpKpi.CantidadTerminadoJP;
               
                //Contar incidencias Con QA, rechazos y rechazos sin idmh
                jpKpi.ConQACount = datos
                    .Where(x => x.JefeDeProyecto == jefeProyecto && x.Type != "Rechazo") 
                    //&& x.FechaTerminoReal != null && DateTime.Parse(x.FechaTerminoReal).Year == 2024)
                    .Select(x => new
                    {
                        IdMH = x.IDMh,
                        IdReadable = x.idReadable
                    })
                .Count();

                //Toma los rechazos que tengan asignado un IDMH
                jpKpi.Rechazos = datos
                    .Where(x => x.JefeDeProyecto == jefeProyecto && x.IDMh != null && x.Type == "Rechazo")
                    //&& x.FechaTerminoReal != null && DateTime.Parse(x.FechaTerminoReal).Year == 2024)
                    .Select(x => new
                    {
                        IdMH = x.IDMh,
                        IdReadable = x.idReadable
                    })
                .Count();

                totalRechazos += jpKpi.Rechazos;

                //Lista de las incidencias con QA, rechazos y rechazos sin idmh
                var proyectosConQA = datos
                    .Where(x => x.JefeDeProyecto == jefeProyecto && x.Type != "Rechazo")
                    //&& x.FechaTerminoReal != null && DateTime.Parse(x.FechaTerminoReal).Year == 2024)
                    .Select(x => new
                    {
                        IdMH = x.IDMh,
                        IdReadable = x.idReadable
                    })
                .ToList();

                var proyectosRechazos = datos
                    .Where(x => x.JefeDeProyecto == jefeProyecto && x.IDMh != null && x.Type == "Rechazo")
                    //&& x.FechaTerminoReal != null && DateTime.Parse(x.FechaTerminoReal).Year == 2024)
                    .Select(x => new
                    {
                        IdMH = x.IDMh,
                        IdReadable = x.idReadable
                    })
                .ToList();

                var proyectosRechazosSinIDMH = datos
                    .Where(x => x.JefeDeProyecto == jefeProyecto && x.IDMh == null && x.Type == "Rechazo")
                    //&& x.FechaTerminoReal != null && DateTime.Parse(x.FechaTerminoReal).Year == 2024)
                    .Select(x => new
                    {
                        IdMH = x.IDMh,
                        IdReadable = x.idReadable
                    })
                .ToList();



                var TipoRechazos = datos
                    .Where(x => x.JefeDeProyecto == jefeProyecto && x.IDMh != null && x.Type == "Rechazo")
                .Select(x => new
                {
                    Type = x.Type,
                    idReadable = x.idReadable,
                    summary = x.summary,
                })
                .ToList();



                //// Imprimir contenido de proyectosConQA
                //Console.WriteLine($"Proyectos con QA para el Jefe de Proyecto {jefeProyecto}:");
                //foreach (var proyecto in proyectosConQA)
                //{
                //    Console.WriteLine($"IdMH: {proyecto.IdMH}, IdReadable: {proyecto.IdReadable}");
                //}

                //// Imprimir contenido de proyectosRechazos
                //Console.WriteLine($"Proyectos de Rechazo para el Jefe de Proyecto {jefeProyecto}:");
                //foreach (var proyecto in proyectosRechazos)
                //{
                //    Console.WriteLine($"IdMH: {proyecto.IdMH}, IdReadable: {proyecto.IdReadable}");
                //}

                jpKpi.QAFallido = datos
                    .Where(x => x.JefeDeProyecto == jefeProyecto && x.State == "Fallido")
                    //&& x.FechaTerminoReal != null && DateTime.Parse(x.FechaTerminoReal).Year == 2024)
                    .Count();

                jpKpi.QAPendiente = datos
                    .Where(x => x.JefeDeProyecto == jefeProyecto && (x.State == "Pendiente" || x.State == "En curso"))
                    //&& x.FechaTerminoReal != null && DateTime.Parse(x.FechaTerminoReal).Year == 2024))
                    .Count();

                jpKpi.QAExitoso = datos
                    .Where(x => x.JefeDeProyecto == jefeProyecto && (x.State == "Exitoso completo" || x.State == "Exitoso liviano"))
                    //&& x.FechaTerminoReal != null && DateTime.Parse(x.FechaTerminoReal).Year == 2024))
                    .Count();

                jpKpi.TotalQA = jpKpi.ConQACount + jpKpi.Rechazos + jpKpi.QAFallido + jpKpi.QAPendiente + jpKpi.QAExitoso;


                //Añade a la lista
                list_jpKpi.Add(jpKpi);

            }

            // Calcular porcentaje de rechazos después de la iteración completa
            foreach (var jpKpi in list_jpKpi)
            {
                // Calcular el porcentaje de rechazos en comparación con el total de proyectos
                jpKpi.PorcentajeRechazos = totalRechazos != 0 ? Math.Round((double)(jpKpi.Rechazos * 100) / totalRechazos, 1) : 0;
            }

            //Asignar la lista de resultados a la propiedad de la clase MetricasKPI
            metricasKPI.kPI_Lista_JPs = list_jpKpi;

            return metricasKPI;
        }


        //Metodo para los modal de Incidencias con QA y los rechazos
        public List<FieldsDTO> DetallesFiltrosQAModal(List<FieldsDTO> metricas, string nomJefeProyecto, string tipo)
        {
            var datos = metricas;

            List<FieldsDTO> jpKpi = new();

            if (tipo == "Rechazos")
            {

                var proyectosRechazos = datos
                    .Where(x => x.JefeDeProyecto == nomJefeProyecto && x.IDMh != null && x.Type == "Rechazo" && x.FechaTerminoReal != null
                                && DateTime.Parse(x.FechaTerminoReal).Year == 2024)
                .Select(x =>
                {
                    x.Subsystem = x.Subsystem;  // Agregar el campo Subsystem sin perder la información existente
                    x.Assignee = x.Assignee;
                    x.IDMh = x.IDMh;
                    x.URLJira = x.URLJira;
                    x.Type = x.Type;
                    x.idReadable = x.idReadable;
                    x.summary = x.summary;
                    x.UrlYT = _configuration["UrlIndicencias"] + x.idReadable + "/" + Uri.EscapeDataString(x.summary);
                    return x;
                })
                .ToList();

                jpKpi.AddRange(proyectosRechazos);
            }
            else if (tipo != "Rechazos")
            {
                var proyectosConQA = datos
                    .Where(x => x.JefeDeProyecto == nomJefeProyecto && x.Type != "Rechazo" && x.FechaTerminoReal != null
                                && DateTime.Parse(x.FechaTerminoReal).Year == 2024)
                    .Select(x =>
                    {
                        x.Subsystem = x.Subsystem;  // Agregar el campo Subsystem sin perder la información existente
                        x.Assignee = x.Assignee;
                        x.IDMh = x.IDMh;
                        x.URLJira = x.URLJira;
                        x.Type = x.Type;
                        x.idReadable = x.idReadable;
                        x.summary = x.summary;
                        x.UrlYT = _configuration["UrlIndicencias"] + x.idReadable + "/" + Uri.EscapeDataString(x.summary);
                        return x;
                    })
                .ToList();

                jpKpi.AddRange(proyectosConQA);
            }
            else if (tipo == "Rechazos Sin Id")
            {
                var proyectosRechazosSinId = datos
                    .Where(x => x.JefeDeProyecto == nomJefeProyecto && x.IDMh == null && x.Type == "Rechazo" && x.FechaTerminoReal != null
                                && DateTime.Parse(x.FechaTerminoReal).Year == 2024)
                .Select(x =>
                {
                    x.Subsystem = x.Subsystem;  // Agregar el campo Subsystem sin perder la información existente
                    x.Assignee = x.Assignee;
                    x.IDMh = x.IDMh;
                    x.URLJira = x.URLJira;
                    x.Type = x.Type;
                    x.idReadable = x.idReadable;
                    x.summary = x.summary;
                    x.UrlYT = _configuration["UrlIndicencias"] + x.idReadable + "/" + Uri.EscapeDataString(x.summary);
                    return x;
                })
                .ToList();

                jpKpi.AddRange(proyectosRechazosSinId);
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
                .Where(x => x.JefeDeProyecto == nomJefeProyecto &&
                                ((x.FechaTerminoDesa != null ? DateTime.Parse(x.FechaTerminoDesa).Date : DateTime.Now.Date) < DateTime.Now.Date) &&
                                (x.State == "En desarrollo" || x.State == "En curso" || x.State == "Pendiente"))
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
                    .Where(x => x.JefeDeProyecto == nomJefeProyecto &&
                                ((x.FechaTerminoQA != null ? DateTime.Parse(x.FechaTerminoQA).Date : DateTime.Now.Date) < DateTime.Now.Date) &&
                                (x.State == "Instalación QA" || x.State == "Exitoso completo" || x.State == "Fallido" || x.State == "Exitoso liviano" || x.State == "Pruebas" || x.State == "Pruebas PROSYS"))
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
                .Where(x => x.JefeDeProyecto == nomJefeProyecto &&
                                ((x.FechaTerminoReal != null ? DateTime.Parse(x.FechaTerminoReal).Date : DateTime.Now.Date) < DateTime.Now.Date) &&
                                (x.State == "Instalación producción" || x.State == "Certificación producción"))
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
                .Where(x => x.JefeDeProyecto == nomJefeProyecto && (x.State == "En desarrollo" || x.State == "Pendiente" || x.State == "Detenido" || x.State == "En curso"
                || x.State == "Instalación QA" || x.State == "Exitoso completo" || x.State == "Fallido" || x.State == "Exitoso liviano" || x.State == "Pruebas" || x.State == "Pruebas PROSYS"
                || x.State == "Instalación producción" || x.State == "Certificación producción"))
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
            else if (tipo == "Producción")
            {
                var jpKpiEnCurso = datos
                .Where(x => x.JefeDeProyecto == nomJefeProyecto && (x.State == "Instalación producción" || x.State == "Certificación producción"))
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


        //Metodo para summary tipo rechazo
        public async Task<List<string>> ObtenerTiposRechazos(List<FieldsDTO> metricas)
        {

            // Lista para almacenar los tipos de rechazos
            List<string> tiposRechazos = new List<string>();

            foreach (FieldsDTO res in metricas)
            {
                if (res.summary != null)
                {
                    string summary_ = res.summary;

                    // Agrega a la lista si aún no está presente
                    if (!tiposRechazos.Contains(summary_))
                    {
                        tiposRechazos.Add(summary_);
                    }
                }
            }

            return tiposRechazos;
        }

        //Metodo para obtener los tipos de rechazos
        public async Task<MetricasKPI> TiposRechazos (List<FieldsDTO> metricas, MetricasKPI metricasKPI)
        {
            // Obtener la lista de los tipos de rechazos
            List<string> tiposRechazos = await ObtenerTiposRechazos(metricas);

            //Obtener datos
            var datos = metricas;


            //Lista para almacenar resultados por rechazos
            List<T_Rechazos> list_jpKpi = new();

            //// Calcular totales por cada jefe de proyecto
            //foreach (var summary_ in tiposRechazos)
            //{
            //    T_Rechazos jpKpi = new();

            //    jpKpi.NomRechazo = summary_;
            //    jpKpi.TipoRechazosCount = datos.Where(x => x.summary == summary_).Count();

            //    var TipoRechazosList = datos
            //        .Where(x => x.summary == summary_ && x.IDMh != null && x.Type == "Rechazo")
            //    .Select(x => new
            //    {
            //        Type = x.Type,
            //        idReadable = x.idReadable,
            //        summary = x.summary,
            //        NomJP = x.JefeDeProyecto
            //    })
            //    .ToList();

            //    jpKpi.TipoRechazos = datos
            //        .Where(x => x.summary == summary_ && x.IDMh != null && x.Type == "Rechazo")
            //    .Select(x => new
            //    {
            //        Type = x.Type,
            //        idReadable = x.idReadable,
            //        summary = x.summary,
            //        NomJP = x.JefeDeProyecto
            //    })
            //    .Count();

            //    //Añade a la lista
            //    list_jpKpi.Add(jpKpi);

            //}

            foreach (var summary_ in tiposRechazos)
            {
                // Verificar si el tipo de rechazo es "Rechazo"
                if (datos.Any(x => x.summary == summary_ && x.Type == "Rechazo"))
                {
                    T_Rechazos jpKpi = new();

                    // Asignar NomRechazo solo si el tipo de rechazo es "Rechazo"
                    jpKpi.NomRechazo = summary_;

                    jpKpi.TipoRechazosCount = datos.Count(x => x.summary == summary_ && x.Type == "Rechazo");

                    // Obtener la lista de rechazos específicos para este tipo de rechazo
                    var TipoRechazosList = datos
                        .Where(x => x.summary == summary_ && x.Type == "Rechazo" && x.IDMh != null)
                        .Select(x => new
                        {
                            Type = x.Type,
                            idReadable = x.idReadable,
                            summary = x.summary,
                            NomJP = x.JefeDeProyecto
                        })
                        .ToList();

                    // Obtener el recuento de rechazos para este tipo
                    jpKpi.TipoRechazos = TipoRechazosList.Count;

                    // Añadir a la lista
                    list_jpKpi.Add(jpKpi);
                }
            }


            //Asignar la lista de resultados a la propiedad de la clase MetricasKPI
            metricasKPI.T_RechazosT = list_jpKpi;

            return metricasKPI;
        }

    }
}


