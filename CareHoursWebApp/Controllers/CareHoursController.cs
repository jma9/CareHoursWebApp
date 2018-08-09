using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CareHoursWebApp.Models;
using CareHoursWebApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CareHoursWebApp.Controllers
{
    public class CareHoursController : Controller
    {
        private readonly ICareHoursService _careHoursService;

        public CareHoursController(ICareHoursService careHoursService)
        {
            _careHoursService = careHoursService;
        }

        // GET: CareHours/Create
        public ActionResult Create(int id)
        {
            ViewBag.ChildId = id;
            return View();
        }

        // POST: CareHours/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ChildId,StartTime,EndTime")] CareHours careHours)
        {
            if (ModelState.IsValid)
            {
                await _careHoursService.CreateAsync(careHours);
                return RedirectToAction(nameof(ChildrenController.Details), "Children", new { id = careHours.ChildId });
            }
            return View();
        }

        // GET: CareHours/Delete/5
        public async Task<IActionResult> Delete(int? id, int childId)
        {
            if (id == null)
            {
                return NotFound();
            }

            var careHours = await _careHoursService.GetAsync(childId, id.Value);
            if (careHours == null)
            {
                return NotFound();
            }

            return View(careHours);
        }

        // POST: CareHours/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int eventId, int childId)
        {
            var careHours = await _careHoursService.GetAsync(childId, eventId);
            await _careHoursService.DeleteAsync(careHours);
            return RedirectToAction(nameof(ChildrenController.Details), "Children", new { id = careHours.ChildId });
        }
    }
}