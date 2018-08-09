using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CareHoursWebApp.Models;
using System.Net.Http;
using System.Web;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;
using System.Runtime.Serialization;
using Microsoft.Extensions.Options;
using CareHoursWebApp.Services;

namespace CareHoursWebApp.Controllers
{
    public class ChildrenController : Controller
    {
        private readonly IChildrenService _childrenService;

        public ChildrenController(IChildrenService childrenService)
        {
            _childrenService = childrenService;
        }

        // GET: Children
        public async Task<IActionResult> Index()
        {
            return View(await _childrenService.GetListAsync());
        }

        // GET: Children/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var child = await _childrenService.GetAsync(id.Value);
            if (child == null)
            {
                return NotFound();
            }

            return View(child);
        }

        // GET: Children/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Children/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ChildId,FirstName,LastName")] Child child)
        {
            if (ModelState.IsValid)
            {
                await _childrenService.CreateAsync(child);
                return RedirectToAction(nameof(Index));
            }
            return View(child);
        }

        // GET: Children/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var child = await _childrenService.GetAsync(id.Value);
            if (child == null)
            {
                return NotFound();
            }
            return View(child);
        }

        // POST: Children/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("ChildId,FirstName,LastName")] Child child)
        {
            if (ModelState.IsValid)
            {
                await _childrenService.UpdateAsync(child);
                return RedirectToAction(nameof(Index));
            }
            return View(child);
        }

        // GET: Children/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var child = await _childrenService.GetAsync(id.Value);
            if (child == null)
            {
                return NotFound();
            }

            return View(child);
        }

        // POST: Children/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int childId)
        {
            var child = await _childrenService.GetAsync(childId);
            await _childrenService.DeleteAsync(child);
            return RedirectToAction(nameof(Index));
        }
    }
}
