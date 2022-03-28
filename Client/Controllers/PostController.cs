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
    public class PostController : Controller
    {

        string Baseurl = "https://localhost:44305";

        // GET: CategoryController/Create
        public ActionResult Create()
        {
            return View();
        }


        // GET: CategoryController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {

            Post post = new Post();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("/api/posts/" + id);
                if (Res.IsSuccessStatusCode)
                {
                    var postResponse = Res.Content.ReadAsStringAsync().Result;
                    if (postResponse != "")
                        post = JsonConvert.DeserializeObject<Post>(postResponse);

                    return View(post);
                }

            }
            return View(post);

        }

    }
}
