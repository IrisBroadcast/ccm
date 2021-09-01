using CCM.StatisticsWeb.Models;
using CCM.StatisticsWeb.Pages;
using CCM.StatisticsWeb.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace CCM.StatisticsWeb.Services
{
    public class StatisticsDataService : IStatisticsDataService
    {
        private readonly HttpClient _httpClient;

        public StatisticsDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<CodecType>> GetCodecTypes()
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<CodecType>>
                (await _httpClient.GetStreamAsync("api/statistics/getcodectypes"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<IEnumerable<SipAccount>> GetSipAccounts()
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<SipAccount>>
                (await _httpClient.GetStreamAsync("api/statistics/getsipaccounts"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<IEnumerable<DateBasedStatistics>> GetCodecTypeStatistics(Guid codecTypeId, DateTime startTime, DateTime endTime)
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<DateBasedStatistics>>
                (await _httpClient.GetStreamAsync($"api/statistics/getcodectypestatistics?startTime={startTime}&endTime={endTime}&codecTypeId={codecTypeId}"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<IEnumerable<LocationBasedStatistics>> GetLocationBasedStatistics(Guid regionId, Guid ownerId, Guid codecTypeId, DateTime startTime, DateTime endTime)
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<LocationBasedStatistics>>
                (await _httpClient.GetStreamAsync($"api/statistics/getlocationbasedstatistics?regionId={regionId}&codecTypeId={codecTypeId}&ownerId={ownerId }&startTime={startTime}&endTime={ endTime} "), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        //public async Task<LocationStatisticsOverview> GetLocationNumberOfCallsTable(Guid regionId, Guid ownerId, Guid codecTypeId, DateTime startTime, DateTime endTime)
        //{
        //    var a = await JsonSerializer.DeserializeAsync<LocationStatisticsOverview>
        //        (await _httpClient.GetStreamAsync($"api/statistics/getlocationnumberofcallstable?regionId={regionId}&codecTypeId={codecTypeId}&ownerId={ownerId }&startTime={startTime}&endTime={ endTime} "), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        //    return a;
        //}
        public async Task<LocationStatisticsOverview> GetLocationNumberOfCallsTable(Guid regionId, Guid ownerId, DateTime startTime, DateTime endTime)
        {
            var a = await JsonSerializer.DeserializeAsync<LocationStatisticsOverview>
                (await _httpClient.GetStreamAsync($"api/statistics/getlocationnumberofcallstable?regionId={regionId}&ownerId={ownerId }&startTime={startTime}&endTime={ endTime} "), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            return a;
        }

        public async Task<IEnumerable<Owner>> GetOwners()
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<Owner>>
               (await _httpClient.GetStreamAsync("api/statistics/getowners"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<IEnumerable<Region>> GetRegions()
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<Region>>
               (await _httpClient.GetStreamAsync("api/statistics/getregions"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<IEnumerable<DateBasedStatistics>> GetRegionStatistics(Guid regionId, DateTime startTime, DateTime endTime)
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<DateBasedStatistics>>
                (await _httpClient.GetStreamAsync($"api/statistics/getregionstatistics?regionId={regionId}&startTime={startTime}&endTime={endTime}"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<IEnumerable<DateBasedStatistics>> GetAccountStatistics(Guid sipId, DateTime startTime, DateTime endTime)
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<DateBasedStatistics>>
                (await _httpClient.GetStreamAsync($"api/statistics/GetAccountStatistics?sipId={sipId}&startTime={startTime}&endTime={endTime}"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<IEnumerable<DateBasedCategoryStatistics>> GetCategories(DateTime startTime, DateTime endTime)
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<DateBasedCategoryStatistics>>
                (await _httpClient.GetStreamAsync($"api/statistics/getcategorystatistics?startTime={startTime}&endTime={endTime}"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }
    }
}
