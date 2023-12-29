using YoutrackReport.DTOs;
using YoutrackReport.Pages;

namespace YoutrackReport.Servicios.Contrato
{
    public interface IObtieneInformacionCovidService
    {
        Task<List<DatosHistoricos>> ObtieneInformacionCovid();
    }
}
