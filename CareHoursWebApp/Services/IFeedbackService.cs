using System.Threading.Tasks;
using CareHoursWebApp.Models;

namespace CareHoursWebApp.Services
{
    public interface IFeedbackService
    {
        Task<FeedbackResponse> PostFeedback(Feedback feedback);
    }
}