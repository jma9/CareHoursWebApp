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
        private Api api = new Api();

        public ChildrenController()
        {
        }

        private class Api
        {
            private const string API_SUBSCRIPTION_KEY = "{subscription key}";
            private const string JSON_CONTENT_TYPE = "application/json";
            private const string CHILDREN_BASE_URI = "https://jma.azure-api.net/api/child/";
            private const string SUBSCRIPTION_KEY_HEADER = "Ocp-Apim-Subscription-Key";

            HttpClient client = new HttpClient();

            private String JsonSerializeChild(Child child)
            {
                var childSerializer = new DataContractJsonSerializer(typeof(Child));
                var ms = new MemoryStream();
                childSerializer.WriteObject(ms, child);
                byte[] json = ms.ToArray();
                return Encoding.UTF8.GetString(json, 0, json.Length);
            }

            private Child JsonDeserializeChild(String jsonChild)
            {
                var childSerializer = new DataContractJsonSerializer(typeof(Child));
                return childSerializer.ReadObject(new MemoryStream(Encoding.UTF8.GetBytes(jsonChild))) as Child;
            }

            private List<Child> JsonDeserializeChildList(String jsonChild)
            {
                var childListSerializer = new DataContractJsonSerializer(typeof(List<Child>));
                return childListSerializer.ReadObject(new MemoryStream(Encoding.UTF8.GetBytes(jsonChild))) as List<Child>;
            }

            public Api()
            {
                client.DefaultRequestHeaders.Add(SUBSCRIPTION_KEY_HEADER, API_SUBSCRIPTION_KEY);
            }

            public IEnumerable<Child> GetList()
            {
                return JsonDeserializeChildList(client.GetAsync(CHILDREN_BASE_URI).Result.Content.ReadAsStringAsync().Result);
            }

            public Child Get(int childId)
            {
                return JsonDeserializeChild(client.GetAsync(CHILDREN_BASE_URI + childId).Result.Content.ReadAsStringAsync().Result);
            }

            public void Create(Child child)
            {
                var jsonResponse = client.PostAsync(CHILDREN_BASE_URI,
                    new StringContent(JsonSerializeChild(child), Encoding.UTF8, JSON_CONTENT_TYPE)).Result.Content.ReadAsStringAsync().Result;

            }

            public void Update(Child child)
            {
                var jsonResponse = client.PutAsync(CHILDREN_BASE_URI + child.ChildId,
                    new StringContent(JsonSerializeChild(child), Encoding.UTF8, JSON_CONTENT_TYPE)).Result.Content.ReadAsStringAsync().Result;
            }

            public void Delete(Child child)
            {
                var jsonResponse = client.DeleteAsync(CHILDREN_BASE_URI + child.ChildId).Result.Content.ReadAsStringAsync().Result;
            }
        }

        // GET: Children
        public IActionResult Index()
        {
            return View(api.GetList());
        }

        // GET: Children/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var child = api.Get(id.Value);
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
                api.Create(child);
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

            var child = api.Get(id.Value);
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
                api.Update(child);
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

            var child = api.Get(id.Value);
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
            var child = api.Get(id);
            api.Delete(child);
            return RedirectToAction(nameof(Index));
        }
    }
}
