using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CareHoursWebApp.Models;
using CareHoursWebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace CareHoursWebApp.Controllers
{
    public class FeedbackController : Controller
    {
        private IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FeedbackText")] Feedback feedback)
        {
            var feedbackResponse = await _feedbackService.PostFeedback(feedback);
            return View("Response", feedbackResponse);
        }
    }
}