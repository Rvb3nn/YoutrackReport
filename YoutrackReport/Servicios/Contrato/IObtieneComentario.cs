using YoutrackReport.DTOs;

namespace YoutrackReport.Servicios.Contrato
{
    public interface IObtieneComentario
    {
        Task<List<ComentarioDTO>> ObtieneComentarios();
    }
}
