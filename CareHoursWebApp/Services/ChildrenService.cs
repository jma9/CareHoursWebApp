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
        private const string BASE_URI = "https://jma.azure-api.net/api/";
        private const string CHILD_GET_LIST_URI = BASE_URI + "child";
        private const string CHILD_GET_URI = BASE_URI + "child/{0}";
        private const string CHILD_CREATE_URI = BASE_URI + "child";
        private const string CHILD_DELETE_URI = BASE_URI + "child/{0}";
        private const string CHILD_UPDATE_URI = BASE_URI + "child/{0}";

        private const string SUBSCRIPTION_KEY_HEADER = "Ocp-Apim-Subscription-Key";
        private const string SUBSCRIPTION_KEY_CONFIGURATION = "AppSettings:SubscriptionKey";

        private HttpClient client = new HttpClient();

        private JsonSerializer<List<Child>> childListSerializer = new JsonSerializer<List<Child>>();
        private JsonSerializer<Child> childSerializer = new JsonSerializer<Child>();

        public ChildrenService(IConfiguration configuration)
        {
            client.DefaultRequestHeaders.Add(SUBSCRIPTION_KEY_HEADER, configuration[SUBSCRIPTION_KEY_CONFIGURATION]);
        }

        public async Task<IEnumerable<Child>> GetListAsync()
        {
            var response = await client.GetAsync(CHILD_GET_LIST_URI);
            return childListSerializer.Deserialize(await response.Content.ReadAsStringAsync());
        }

        public async Task<Child> GetAsync(int childId)
        {
            var uri = String.Format(CHILD_GET_URI, childId);
            var response = await client.GetAsync(uri);
            return childSerializer.Deserialize(await response.Content.ReadAsStringAsync());
        }

        public async Task<Child> CreateAsync(Child child)
        {
            var response = await client.PostAsync(CHILD_CREATE_URI, childSerializer.JsonHttpStringContent(child));
            var jsonResponse = await response.Content.ReadAsStringAsync();
            return child;
        }

        public async Task<Child> UpdateAsync(Child child)
        {
            var uri = String.Format(CHILD_UPDATE_URI, child.ChildId);
            var response = await client.PutAsync(uri, childSerializer.JsonHttpStringContent(child));
            var jsonResponse = await response.Content.ReadAsStringAsync();
            return child;
        }

        public async Task DeleteAsync(Child child)
        {
            var uri = String.Format(CHILD_DELETE_URI, child.ChildId);
            var response = await client.DeleteAsync(uri);
            var jsonResponse = await response.Content.ReadAsStringAsync();
        }
    }
}
