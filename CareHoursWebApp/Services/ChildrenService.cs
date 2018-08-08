using CareHoursWebApp.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace CareHoursWebApp.Services
{
    public class ChildrenService : IChildrenService
    {
        private const string JSON_CONTENT_TYPE = "application/json";
        private const string CHILDREN_BASE_URI = "https://jma.azure-api.net/api/child/";
        private const string SUBSCRIPTION_KEY_HEADER = "Ocp-Apim-Subscription-Key";

        private HttpClient client = new HttpClient();

        public ChildrenService(IConfiguration configuration)
        {
            client.DefaultRequestHeaders.Add(SUBSCRIPTION_KEY_HEADER, configuration["AppSettings:SubscriptionKey"]);
        }

        private String JsonSerializeChild(Child child)
        {
            var childSerializer = new DataContractJsonSerializer(typeof(Child));
            var ms = new MemoryStream();
            childSerializer.WriteObject(ms, child);
            byte[] json = ms.ToArray();
            return Encoding.UTF8.GetString(json, 0, json.Length);
        }

        private Child JsonDeserializeChild(String jsonChild)
        {
            var childSerializer = new DataContractJsonSerializer(typeof(Child));
            return childSerializer.ReadObject(new MemoryStream(Encoding.UTF8.GetBytes(jsonChild))) as Child;
        }

        private List<Child> JsonDeserializeChildList(String jsonChild)
        {
            var childListSerializer = new DataContractJsonSerializer(typeof(List<Child>));
            return childListSerializer.ReadObject(new MemoryStream(Encoding.UTF8.GetBytes(jsonChild))) as List<Child>;
        }

        public async Task<IEnumerable<Child>> GetListAsync()
        {
            var response = await client.GetAsync(CHILDREN_BASE_URI);
            return JsonDeserializeChildList(await response.Content.ReadAsStringAsync());
        }

        public async Task<Child> GetAsync(int childId)
        {
            var response = await client.GetAsync(CHILDREN_BASE_URI + childId);
            return JsonDeserializeChild(await response.Content.ReadAsStringAsync());
        }

        public async Task<Child> CreateAsync(Child child)
        {
            var response = await client.PostAsync(CHILDREN_BASE_URI,
                new StringContent(JsonSerializeChild(child), Encoding.UTF8, JSON_CONTENT_TYPE));
            var jsonResponse = await response.Content.ReadAsStringAsync();
            return child;
        }

        public async Task<Child> UpdateAsync(Child child)
        {
            var response = await client.PutAsync(CHILDREN_BASE_URI + child.ChildId,
                new StringContent(JsonSerializeChild(child), Encoding.UTF8, JSON_CONTENT_TYPE));
            var jsonResponse = await response.Content.ReadAsStringAsync();
            return child;
        }

        public async Task DeleteAsync(Child child)
        {
            var response = await client.DeleteAsync(CHILDREN_BASE_URI + child.ChildId);
            var jsonResponse = await response.Content.ReadAsStringAsync();
        }
    }
}
