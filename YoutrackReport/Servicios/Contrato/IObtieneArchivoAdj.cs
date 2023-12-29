using YoutrackReport.DTOs;

namespace YoutrackReport.Servicios.Contrato
{
    public interface IObtieneArchivoAdj
    {
        Task<List<ArchivoAdjDTO>> ObtieneArchivos();
    }
}
