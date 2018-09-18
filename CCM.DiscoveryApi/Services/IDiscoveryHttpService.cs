using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using CCM.Core.Discovery;

namespace CCM.DiscoveryApi.Services
{
    public interface IDiscoveryHttpService
    {
        Task<List<FilterDto>> GetFiltersAsync(HttpRequestMessage originalRequest);
        Task<List<ProfileDto>> GetProfilesAsync(HttpRequestMessage originalRequest);
        Task<UserAgentsResultDto> GetUserAgentsAsync(UserAgentSearchParamsDto searchParams, HttpRequestMessage originalRequest);
    }
}