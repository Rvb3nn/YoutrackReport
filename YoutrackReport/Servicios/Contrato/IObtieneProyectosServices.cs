using YoutrackReport.DTOs;

namespace YoutrackReport.Servicios.Contrato
{
    public interface IObtieneProyectosServices
    {
        Task<List<ProyectosDTO>> ObtieneProyectos();
    }
}
