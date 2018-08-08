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
    public class FeedbackService : IFeedbackService
    {
        private const string JSON_CONTENT_TYPE = "application/json";
        private const string FEEDBACK_BASE_URI = "https://jma.azure-api.net/api/feedback";
        private const string SUBSCRIPTION_KEY_HEADER = "Ocp-Apim-Subscription-Key";

        private HttpClient client = new HttpClient();

        private String JsonSerializeFeedback(Feedback feedback)
        {
            var feedbackSerializer = new DataContractJsonSerializer(typeof(Feedback));
            var ms = new MemoryStream();
            feedbackSerializer.WriteObject(ms, feedback);
            byte[] json = ms.ToArray();
            return Encoding.UTF8.GetString(json, 0, json.Length);
        }

        private FeedbackResponse JsonDeserializeFeedbackResponse(String jsonFeedbackResponse)
        {
            var feedbackResponseSerializer = new DataContractJsonSerializer(typeof(FeedbackResponse));
            return feedbackResponseSerializer.ReadObject(new MemoryStream(Encoding.UTF8.GetBytes(jsonFeedbackResponse))) as FeedbackResponse;
        }

        public FeedbackService(IConfiguration configuration)
        {
            client.DefaultRequestHeaders.Add(SUBSCRIPTION_KEY_HEADER, configuration["AppSettings:SubscriptionKey"]);
        }

        public async Task<FeedbackResponse> PostFeedback(Feedback feedback)
        {
            var response = await client.PostAsync(FEEDBACK_BASE_URI,
                new StringContent(JsonSerializeFeedback(feedback), Encoding.UTF8, JSON_CONTENT_TYPE));
            return JsonDeserializeFeedbackResponse(await response.Content.ReadAsStringAsync());
        }
    }
}
