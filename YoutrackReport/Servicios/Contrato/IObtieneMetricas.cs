using System.Collections.Generic;
using System.Threading.Tasks;
using YoutrackReport.DTOs;

namespace YoutrackReport.Servicios.Contrato
{
    public interface IObtieneMetricas
    {
        Task<List<MetricasDTO>> ObtieneMetricasV();
    }
}
