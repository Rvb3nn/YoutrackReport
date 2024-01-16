using System.Collections.Generic;
using System.Threading.Tasks;
using YoutrackReport.DTOs;
using static YoutrackReport.Pages.Metricas;

namespace YoutrackReport.Servicios.Contrato
{
    public interface IObtieneMetricas
    {
        IAsyncEnumerable<MetricasKPI> CalcularTotales();
        IAsyncEnumerable<string> CalcularTotalesPorJP();

        Task<List<string>> ObtenerJefesProyectoUnicos();
    }
}
