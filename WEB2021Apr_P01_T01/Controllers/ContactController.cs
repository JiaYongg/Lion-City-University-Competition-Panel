using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEB2021Apr_P01_T01.Models;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;

namespace WEB2021Apr_P01_T01.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(Contact c)
        {
            if (!ModelState.IsValid) // validation fails
            {
                return View(c);
            }
            else
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("https://webassg2contact-c614.restdb.io");
                client.DefaultRequestHeaders.Add("cache-control", "no-cache");
                client.DefaultRequestHeaders.Add("x-apikey", "44690e8e37335dca37edd43c755a7a762715a");

                string json = JsonConvert.SerializeObject(c);
                StringContent contactContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("/rest/contact-form", contactContent);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Message"] = "We have received your message and will get back to you within 3-5 working days";
                }
                else
                {
                    TempData["Message"] = "Oops, something went wrong. Please try again later";
                }

                return View();
            }
        }
    }
}
