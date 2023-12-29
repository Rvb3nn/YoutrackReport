
using YoutrackReport.DTOs;

namespace YoutrackReport.Servicios.Contrato
{
    public interface IObtieneActivity
    {
        Task<List<ActivityDTO>> ObtieneActivities();
    }
}