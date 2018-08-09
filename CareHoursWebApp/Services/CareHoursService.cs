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
        private const string BASE_URI = "https://jma.azure-api.net/api/";
        private const string CAREHOURS_GET_URI = BASE_URI + "child/{0}/Carehours";
        private const string CAREHOURS_CREATE_URI = BASE_URI + "child/{0}/Carehours";
        private const string CAREHOURS_DELETE_URI = BASE_URI + "child/{0}/Carehours/{1}";

        private const string SUBSCRIPTION_KEY_HEADER = "Ocp-Apim-Subscription-Key";
        private const string SUBSCRIPTION_KEY_CONFIGURATION = "AppSettings:SubscriptionKey";

        private HttpClient client = new HttpClient();

        private JsonSerializer<CareHours> careHoursSerializer = new JsonSerializer<CareHours>();
        private JsonSerializer<List<CareHours>> careHoursListSerializer = new JsonSerializer<List<CareHours>>();

        public CareHoursService(IConfiguration configuration)
        {
            client.DefaultRequestHeaders.Add(SUBSCRIPTION_KEY_HEADER, configuration[SUBSCRIPTION_KEY_CONFIGURATION]);
        }

        public async Task<IEnumerable<CareHours>> GetCareHoursForChildAsync(int childId)
        {
            var uri = String.Format(CAREHOURS_GET_URI, childId);
            var response = await client.GetAsync(uri);
            var jsonResponse = await response.Content.ReadAsStringAsync();
            return careHoursListSerializer.Deserialize(jsonResponse);
        }

        public async Task<CareHours> CreateAsync(CareHours careHours)
        {
            var uri = String.Format(CAREHOURS_CREATE_URI, careHours.ChildId);
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
            var uri = String.Format(CAREHOURS_DELETE_URI, careHours.ChildId, careHours.EventId);
            var response = await client.DeleteAsync(uri);
            var jsonResponse = await response.Content.ReadAsStringAsync();
        }
    }
}
