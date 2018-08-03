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

namespace CareHoursWebApp.Controllers
{
    public class ChildrenController : Controller
    {
        public ChildrenController()
        {
        }

        private String Get(string uri)
        {
            var client = new HttpClient();
            //            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "{subscription key}");
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "e9149432203547528d11b5749f0f4278");
            return client.GetAsync(uri).Result.Content.ReadAsStringAsync().Result;
        }

        private IEnumerable<Child> Children()
        {
            var json = Get("https://jma.azure-api.net/api/child");

            var ser = new DataContractJsonSerializer(typeof(List<List<Child>>));
            var children = (ser.ReadObject(new MemoryStream(Encoding.UTF8.GetBytes(json))) as List<List<Child>>).First();

            return children;
        }

        private Child Child(int childId)
        {
            var json = Get("https://jma.azure-api.net/api/child/" + childId);
            var ser = new DataContractJsonSerializer(typeof(Child));
            var child = ser.ReadObject(new MemoryStream(Encoding.UTF8.GetBytes(json))) as Child;

            return child;
        }

        private Child AddChild(Child child)
        {
            return child;
        }

        private void UpdateChild(Child child)
        {
        }

        private void RemoveChild(Child child)
        {
        }

        // GET: Children
        public IActionResult Index()
        {
            return View(Children());
        }

        // GET: Children/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var child = Child(id.Value);
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
        public IActionResult Create([Bind("ChildId,FirstName,LastName")] Child child)
        {
            if (ModelState.IsValid)
            {
                AddChild(child);
                return RedirectToAction(nameof(Index));
            }
            return View(child);
        }

        // GET: Children/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var child = Child(id.Value);
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
        public IActionResult Edit(int id, [Bind("ChildId,FirstName,LastName")] Child child)
        {
            if (id != child.ChildId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                UpdateChild(child);
                return RedirectToAction(nameof(Index));
            }
            return View(child);
        }

        // GET: Children/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var child = Child(id.Value);
            if (child == null)
            {
                return NotFound();
            }

            return View(child);
        }

        // POST: Children/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var child = Child(id);
            RemoveChild(child);
            return RedirectToAction(nameof(Index));
        }
    }
}
