using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Text.Json;
using TrackerService.Domain.Entities;
using TrackerService.Domain.Interface;


namespace TrackerService.Infrastructure
{
    public class PeopleServiceClient : IPeopleServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<PeopleServiceClient> _logger;

        public PeopleServiceClient(HttpClient httpClient, ILogger<PeopleServiceClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        //public async Task<List<TrackPerson>> GetAllPeopleAsync(string token)
        //{
        //    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        //    var response = await _httpClient.GetAsync("api/People");

        //    if (!response.IsSuccessStatusCode)
        //    {
        //        _logger.LogError("Failed to fetch people. StatusCode: {Code}", response.StatusCode);
        //        return new List<TrackPerson>();
        //    }

        //    var content = await response.Content.ReadAsStringAsync();
        //    return JsonSerializer.Deserialize<List<TrackPerson>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
        //}
        public async Task<List<TrackPerson>> GetAllPeopleAsync(string token)
        {
            // ✅ Step 1: Clean the token (avoid "Bearer Bearer" problem)
            if (token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                token = token.Substring("Bearer ".Length);

            // ✅ Step 2: Add Authorization header
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            _logger.LogInformation("Calling PeopleService with token: {Token}", token);

            // ✅ Step 3: Make request
            var response = await _httpClient.GetAsync("api/People");

            // ❌ Step 4: Check response
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("❌ Failed to fetch people. StatusCode: {Code}, Reason: {Reason}", response.StatusCode, response.ReasonPhrase);

                // Optional: log raw content to debug error message
                string rawContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("Raw response body: {Body}", rawContent);

                return new List<TrackPerson>();
            }

            // ✅ Step 5: Deserialize JSON result
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<TrackPerson>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
        }
        public async Task<TrackPerson?> GetPersonByIdAsync(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync($"api/people/{id}");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Person with ID {Id} not found", id);
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TrackPerson>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}
