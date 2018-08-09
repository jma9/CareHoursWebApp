using CareHoursWebApp.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CareHoursWebApp.Services
{
    public class CareHoursService : ICareHoursService
    {
        private const string SUBSCRIPTION_KEY_HEADER = "Ocp-Apim-Subscription-Key";
        private const string SUBSCRIPTION_KEY_CONFIGURATION = "AppSettings:SubscriptionKey";
        private const string API_BASE_URI_CONFIGURATION = "AppSettings:ApiBaseUri";

        private readonly Uri baseUri;
        private readonly HttpClient client = new HttpClient();

        private readonly JsonSerializer<CareHours> careHoursSerializer = new JsonSerializer<CareHours>();
        private readonly JsonSerializer<List<CareHours>> careHoursListSerializer = new JsonSerializer<List<CareHours>>();

        public CareHoursService(IConfiguration configuration)
        {
            client.DefaultRequestHeaders.Add(SUBSCRIPTION_KEY_HEADER, configuration[SUBSCRIPTION_KEY_CONFIGURATION]);
            baseUri = new Uri(configuration[API_BASE_URI_CONFIGURATION]);
        }

        public async Task<IEnumerable<CareHours>> GetCareHoursForChildAsync(int childId)
        {
            var uri = new Uri(baseUri, $"/api/child/{childId}/Carehours");

            var response = await client.GetAsync(uri);
            var jsonResponse = await response.Content.ReadAsStringAsync();
            return careHoursListSerializer.Deserialize(jsonResponse);
        }

        public async Task<CareHours> CreateAsync(CareHours careHours)
        {
            var uri = new Uri(baseUri, $"/api/child/{careHours.ChildId}/Carehours");

            var response = await client.PostAsync(uri, careHoursSerializer.JsonHttpStringContent(careHours));
            var jsonResponse = await response.Content.ReadAsStringAsync();
            return careHours;
        }

        public async Task<CareHours> GetAsync(int childId, int eventId)
        {
            var careHoursList = await GetCareHoursForChildAsync(childId);
            return careHoursList.FirstOrDefault(c => c.EventId == eventId);
        }

        public async Task DeleteAsync(CareHours careHours)
        {
            var uri = new Uri(baseUri, $"/api/child/{careHours.ChildId}/Carehours/{careHours.EventId}");

            var response = await client.DeleteAsync(uri);
            var jsonResponse = await response.Content.ReadAsStringAsync();
        }
    }
}
