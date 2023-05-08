using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using FashionStore.Models;
using System;
using Microsoft.IdentityModel.Tokens;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Data;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;

namespace FashionStore.Controllers
{
    [Authorize]
    public class ColorController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7285/api/");
        HttpClient client;

        public ColorController()
        {
            client = new HttpClient();
            client.BaseAddress = baseAddress;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public ActionResult ColorAPIIndex()
        {
            List<ColorModel> _ColorList = new List<ColorModel>();
            var response = client.GetAsync("ColorModels").Result;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var responseData = response.Content.ReadAsStringAsync().Result;
                _ColorList = JsonConvert.DeserializeObject<List<ColorModel>>(responseData);
                Console.WriteLine("inside if ");
            }
            else
            {
                ModelState.AddModelError("", "Error getting data from API.");
            }
            return View(_ColorList);
        }

        // GET: Color/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Color/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ColorModel colorModel)
        {
            try
            {
                var response = client.PostAsJsonAsync("ColorModels", colorModel).Result;
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("ColorAPIIndex");
                }
                else
                {
                    ModelState.AddModelError("", "Error creating color.");
                    return View(colorModel);
                }
            }
            catch
            {
                ModelState.AddModelError("", "Error creating color.");
                return View(colorModel);
            }
        }

        // GET: Color/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                var response = client.GetAsync("ColorModels/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    var responseData = response.Content.ReadAsStringAsync().Result;
                    var colorModel = JsonConvert.DeserializeObject<ColorModel>(responseData);
                    return View(colorModel);
                }
                else
                {
                    ModelState.AddModelError("", "Error getting color.");
                    return View();
                }
            }
            catch
            {
                ModelState.AddModelError("", "Error getting color.");
                return View();
            }
        }

        // POST: Color/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ColorModel colorModel)
        {
            try
            {
                var response = client.PutAsJsonAsync("ColorModels/" + id, colorModel).Result;
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("ColorAPIIndex");
                }
                else
                {
                    ModelState.AddModelError("", "Error updating color.");
                    return View(colorModel);
                }
            }
            catch
            {
                ModelState.AddModelError("", "Error updating color.");
                return View(colorModel);
            }
        }

        // GET: Color/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                var response = client.GetAsync("ColorModels/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    var responseData = response.Content.ReadAsStringAsync().Result;
                    var colorModel = JsonConvert.DeserializeObject<ColorModel>(responseData);
                    return View(colorModel);
                }
                else
                {
                    ModelState.AddModelError("", "Error getting color.");
                    return View();
                }
            }
            catch
            {
                ModelState.AddModelError("", "Error getting color.");
                return View();
            }
        }

        // POST: Color/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id,ColorModel color)
        {
            try
            {
                var response = client.DeleteAsync("ColorModels/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("ColorAPIIndex");
                }
                else
                {
                    ModelState.AddModelError("", "Error deleting color.");
                    return View();
                }
            }
            catch
            {
                ModelState.AddModelError("", "Error deleting color.");
                return View();
            }
        }

        
    }
}
