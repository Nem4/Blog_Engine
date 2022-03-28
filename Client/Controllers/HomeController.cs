using Client.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using System.Threading.Tasks;
using API.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        string Baseurl = "https://localhost:44305";

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {

            HomePageModel homeData = new HomePageModel();


            homeData.Categories = await GetCategoriesAsync();
            homeData.Posts = await GetPostsAsync();
            //returning the employee list to view
            return View(homeData);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }      


        public async Task<List<Category>> GetCategoriesAsync()
        {
            List<Category> Categories = new List<Category>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("/api/categories");
                if (Res.IsSuccessStatusCode)
                {
                    var categoryResponse = Res.Content.ReadAsStringAsync().Result;
                    if(categoryResponse != "")
                        Categories = JsonConvert.DeserializeObject<List<Category>>(categoryResponse);   

                    return Categories;
                }

            }
            return Categories;

        }


        public async Task<List<Post>> GetPostsAsync()
        {
            List<Post> Posts = new List<Post>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("/api/posts");
                if (Res.IsSuccessStatusCode)
                {
                    var postResponse = Res.Content.ReadAsStringAsync().Result;
                    if (postResponse != "")
                        Posts = JsonConvert.DeserializeObject<List<Post>>(postResponse);

                    return Posts;
                }

            }
            return Posts;

        }
    }
}
