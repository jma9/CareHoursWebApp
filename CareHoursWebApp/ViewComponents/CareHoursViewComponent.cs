using CareHoursWebApp.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CareHoursWebApp.ViewComponents
{
    public class CareHoursViewComponent : ViewComponent
    {
        private readonly ICareHoursService _careHoursService;

        public CareHoursViewComponent(ICareHoursService careHoursService)
        {
            _careHoursService = careHoursService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int childId)
        {
            ViewBag.ChildId = childId;
            return View(await _careHoursService.GetCareHoursForChildAsync(childId));
        }
    }
}
