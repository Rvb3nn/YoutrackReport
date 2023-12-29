
using YoutrackReport.DTOs;
using YoutrackReport.Servicios.Contrato;

namespace YoutrackReport.Servicios.Impllementacion
{
    public class ObtieneActivity : IObtieneActivity
    {
        private readonly HttpClient _httpClient;

        public ObtieneActivity(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ActivityDTO>> ObtieneActivities()
        {
            var api = "https://prosysspa.youtrack.cloud/api/issues/2-796/activities?fields=id,author(name,login),timestamp,target(id,text),authorGroup(id,name)&categories=CommentsCategory,IssueCreatedCategory,SummaryCategory,WorkItemCategory,ProjectCategory,IssueVisibilityCategory,AttachmentsCategory";

            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, api);
                request.Headers.Add("Authorization", "Bearer perm:Y2d1dGllcnJleg==.NTgtNw==.ZAD9HryU5yjSvPvNr2BvmeMTyUmTMZ");

                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var activities = await response.Content.ReadFromJsonAsync<List<ActivityDTO>>();

                return activities ?? new List<ActivityDTO>();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
