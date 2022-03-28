using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using API.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Client.Controllers
{
    public class CategoryController : Controller
    {
        string Baseurl = "https://localhost:44305";

        // GET: CategoryController
        public ActionResult Index()
        {
            return View();
        }

        // GET: CategoryController/Create
        public ActionResult Create()
        {
            return View();
        }

        // GET: CategoryController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            Category category = new Category();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("/api/categories/" + id);
                if (Res.IsSuccessStatusCode)
                {
                    var categoryResponse = Res.Content.ReadAsStringAsync().Result;
                    if (categoryResponse != "")
                        category = JsonConvert.DeserializeObject<Category>(categoryResponse);

                    return View(category);
                }

            }
            return View(category);

        }
    }
}
