using YoutrackReport.Pages;

namespace YoutrackReport.DTOs
{
    public class MetricasKPI
    {
        public int CantidadEnCurso { get; set; }
        public int CantidadTerminado { get; set; }
        public int CantidadTerminado2023 { get; set; }
        public int CantidadTerminado2024 { get; set; }

        public List<KPI_Lista_JP> kPI_Lista_JPs { get; set; }
    }

    public class KPI_Lista_JP 
    {
        public string NomJP { get; set; }
        public int CantidadTerminadoJP { get; set; }
        public int CantidadEncursoJP { get; set; }
        public int CantidadDesarrolloJP { get; set; }
        public int CantidadQAJP { get; set; }
        public int CantidadProdJP { get; set; }
        public int TotalAtrasosJP { get; set; }
        public int TotalJP { get; set; }
        public int TotalQA { get; set; }
        public int AtrasoDesarrolloJP { get; set; }
        public int AtrasoQAJP { get; set; }
        public int AtrasoProduccionJP { get; set; }
        public int ConQACount { get; set; }
        public int QAFallido { get; set; }
        public int QAPendiente { get; set; }
        public int QAExitoso { get; set; }
        public int QARealizado { get; set; }
        public int Rechazos { get; set; }
        public int RechazosSinId { get; set; }
        public double PorcentajeRechazos { get; set; }
        public int TotalRechazos { get; set; }

    }

}
