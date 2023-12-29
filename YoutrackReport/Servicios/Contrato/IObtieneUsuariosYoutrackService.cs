using YoutrackReport.DTOs;

namespace YoutrackReport.Servicios.Contrato
{
    public interface IObtieneUsuariosYoutrackService
    {
        Task<List<UsuariosDTO>> ObtieneUsuarios();
    }
}
