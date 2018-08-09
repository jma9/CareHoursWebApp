using CareHoursWebApp.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CareHoursWebApp.Services
{
    public class FeedbackService : IFeedbackService
    {
        private const string SUBSCRIPTION_KEY_HEADER = "Ocp-Apim-Subscription-Key";
        private const string SUBSCRIPTION_KEY_CONFIGURATION = "AppSettings:SubscriptionKey";
        private const string API_BASE_URI_CONFIGURATION= "AppSettings:ApiBaseUri";

        private readonly HttpClient client = new HttpClient();

        private readonly JsonSerializer<Feedback> feedbackSerializer = new JsonSerializer<Feedback>();
        private readonly JsonSerializer<FeedbackResponse> feedbackResponseSerializer = new JsonSerializer<FeedbackResponse>();

        public FeedbackService(IConfiguration configuration)
        {
            client.DefaultRequestHeaders.Add(SUBSCRIPTION_KEY_HEADER, configuration[SUBSCRIPTION_KEY_CONFIGURATION]);
            client.BaseAddress = new Uri(configuration[API_BASE_URI_CONFIGURATION]);
        }

        public async Task<FeedbackResponse> PostFeedback(Feedback feedback)
        {
            var uri = "api/feedback";

            var responseStreamTask = await client.PostAsync(uri, feedbackSerializer.JsonHttpStringContent(feedback)).ContinueWith(t => t.Result.Content.ReadAsStreamAsync());
            return await feedbackResponseSerializer.DeserializeAsync(responseStreamTask);
        }
    }
}
