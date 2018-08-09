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

        private readonly HttpClient client = new HttpClient();

        private readonly JsonSerializer<CareHours> careHoursSerializer = new JsonSerializer<CareHours>();
        private readonly JsonSerializer<List<CareHours>> careHoursListSerializer = new JsonSerializer<List<CareHours>>();

        public CareHoursService(IConfiguration configuration)
        {
            client.DefaultRequestHeaders.Add(SUBSCRIPTION_KEY_HEADER, configuration[SUBSCRIPTION_KEY_CONFIGURATION]);
            client.BaseAddress = new Uri(configuration[API_BASE_URI_CONFIGURATION]);
        }

        public async Task<IEnumerable<CareHours>> GetCareHoursForChildAsync(int childId)
        {
            var uri = $"api/child/{childId}/Carehours";

            var responseStreamTask = client.GetStreamAsync(uri);
            return await careHoursListSerializer.DeserializeAsync(responseStreamTask);
        }

        public async Task<CareHours> CreateAsync(CareHours careHours)
        {
            var uri = $"api/child/{careHours.ChildId}/Carehours";

            var responseStreamTask = await client.PostAsync(uri, careHoursSerializer.JsonHttpStringContent(careHours)).ContinueWith(t => t.Result.Content.ReadAsStreamAsync());
            return careHours;
        }

        public async Task<CareHours> GetAsync(int childId, int eventId)
        {
            var careHoursList = await GetCareHoursForChildAsync(childId);
            return careHoursList.FirstOrDefault(c => c.EventId == eventId);
        }

        public async Task DeleteAsync(CareHours careHours)
        {
            var uri = $"api/child/{careHours.ChildId}/Carehours/{careHours.EventId}";

            var responseStreamTask = await client.DeleteAsync(uri).ContinueWith(t => t.Result.Content.ReadAsStreamAsync());
        }
    }
}
