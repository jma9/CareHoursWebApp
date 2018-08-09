using CareHoursWebApp.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CareHoursWebApp.Services
{
    public class ChildrenService : IChildrenService
    {
        private const string SUBSCRIPTION_KEY_HEADER = "Ocp-Apim-Subscription-Key";
        private const string SUBSCRIPTION_KEY_CONFIGURATION = "AppSettings:SubscriptionKey";
        private const string API_BASE_URI_CONFIGURATION = "AppSettings:ApiBaseUri";

        private readonly HttpClient client = new HttpClient();

        private readonly JsonSerializer<List<Child>> childListSerializer = new JsonSerializer<List<Child>>();
        private readonly JsonSerializer<Child> childSerializer = new JsonSerializer<Child>();

        public ChildrenService(IConfiguration configuration)
        {
            client.DefaultRequestHeaders.Add(SUBSCRIPTION_KEY_HEADER, configuration[SUBSCRIPTION_KEY_CONFIGURATION]);
            client.BaseAddress = new Uri(configuration[API_BASE_URI_CONFIGURATION]);
        }

        public async Task<IEnumerable<Child>> GetListAsync()
        {
            var uri = "api/child";

            var response = await client.GetAsync(uri);
            return childListSerializer.Deserialize(await response.Content.ReadAsStringAsync());
        }

        public async Task<Child> GetAsync(int childId)
        {
            var uri = $"api/child/{childId}";

            var response = await client.GetAsync(uri);
            return childSerializer.Deserialize(await response.Content.ReadAsStringAsync());
        }

        public async Task<Child> CreateAsync(Child child)
        {
            var uri = "api/child";

            var response = await client.PostAsync(uri, childSerializer.JsonHttpStringContent(child));
            var jsonResponse = await response.Content.ReadAsStringAsync();
            return child;
        }

        public async Task<Child> UpdateAsync(Child child)
        {
            var uri = $"api/child/{child.ChildId}";

            var response = await client.PutAsync(uri, childSerializer.JsonHttpStringContent(child));
            var jsonResponse = await response.Content.ReadAsStringAsync();
            return child;
        }

        public async Task DeleteAsync(Child child)
        {
            var uri = $"api/child/{child.ChildId}";

            var response = await client.DeleteAsync(uri);
            var jsonResponse = await response.Content.ReadAsStringAsync();
        }
    }
}
