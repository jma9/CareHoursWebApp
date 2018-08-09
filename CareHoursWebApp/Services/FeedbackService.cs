using CareHoursWebApp.Models;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CareHoursWebApp.Services
{
    public class FeedbackService : IFeedbackService
    {
        private const string BASE_URI = "https://jma.azure-api.net/api/";
        private const string POST_FEEDBACK_URI = BASE_URI + "feedback";

        private const string SUBSCRIPTION_KEY_HEADER = "Ocp-Apim-Subscription-Key";
        private const string SUBSCRIPTION_KEY_CONFIGURATION = "AppSettings:SubscriptionKey";

        private HttpClient client = new HttpClient();

        private JsonSerializer<Feedback> feedbackSerializer = new JsonSerializer<Feedback>();
        private JsonSerializer<FeedbackResponse> feedbackResponseSerializer = new JsonSerializer<FeedbackResponse>();

        public FeedbackService(IConfiguration configuration)
        {
            client.DefaultRequestHeaders.Add(SUBSCRIPTION_KEY_HEADER, configuration[SUBSCRIPTION_KEY_CONFIGURATION]);
        }

        public async Task<FeedbackResponse> PostFeedback(Feedback feedback)
        {
            var response = await client.PostAsync(POST_FEEDBACK_URI, feedbackSerializer.JsonHttpStringContent(feedback));
            return feedbackResponseSerializer.Deserialize(await response.Content.ReadAsStringAsync());
        }
    }
}
