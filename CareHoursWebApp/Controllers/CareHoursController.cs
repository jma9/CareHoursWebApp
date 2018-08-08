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
        public ActionResult Create([Bind("ChildId,StartTime,EndTime")] CareHours careHours)
        {
            if (ModelState.IsValid)
            {
                _careHoursService.Create(careHours);
                return RedirectToAction(nameof(ChildrenController.Details), "Children", new { id = careHours.ChildId });
            }
            return View();
        }

        // GET: CareHours/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CareHours/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(ChildrenController.Details), nameof(ChildrenController), new { childId = 1 });
            }
            catch
            {
                return View();
            }
        }
    }
}