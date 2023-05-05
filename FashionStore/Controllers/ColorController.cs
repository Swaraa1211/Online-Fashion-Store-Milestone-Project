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
        public ActionResult Delete(int id, FormCollection collection)
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

        //Uri baseAddress = new Uri("https://localhost:7285/api/");
        //HttpClient client;

        //public ColorController()
        //{
        //    client = new HttpClient();
        //    client.BaseAddress = baseAddress;
        //}

        //public ActionResult ColorAPIIndex()
        //{
        //    List<ColorModel> _ColorList = new List<ColorModel>();
        //    HttpResponseMessage response = client.GetAsync(client.BaseAddress + "/ColorModels").Result;
        //    if(response.IsSuccessStatusCode)
        //    {
        //        string data = response.Content.ReadAsStringAsync().Result;
        //        _ColorList = JsonConvert.DeserializeObject<List<ColorModel>>(data);  
        //    }
        //    foreach(ColorModel color in _ColorList)
        //    {
        //        Console.WriteLine("Colors: "+color);
        //    }
        //    return View(_ColorList);
        //}

        //public ActionResult ColorAPIIndex()
        //{
        //    IEnumerable<ColorModel> colors;
        //    HttpResponse response = GlobalVariables.WebApiClient.GetAsync("ColorModels").Result;
        //    return View();
        //}


        //IConfiguration _configuration;
        //SqlConnection _connection;

        //public string operationMode = "";

        //public ColorController(IConfiguration configuration)
        //{
        //	_configuration = configuration;
        //	_connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        //}
        //private List<ColorModel> _ColorList = new List<ColorModel>();


        //public void Connection()
        //      {
        //          string? conn = _configuration.GetConnectionString("DefaultConnection");
        //          _connection = new SqlConnection(conn);
        //          _connection.Open();
        //      }
        //[HttpGet]
        //public IActionResult ColorIndex()
        //      {
        //	Connection();
        //	string? opnMode = "Normal";
        //	string colorIdStr = "";

        //	if (Request.Query.ContainsKey("opnMode") && Request.Query.ContainsKey("Color_Id"))
        //	{
        //		opnMode = Request.Query["opnMode"];
        //		if (int.TryParse(Request.Query["Color_Id"], out int colorId))
        //		{
        //			if (opnMode == "Delete")
        //			{
        //				string deleteMeetQuery = $"DELETE FROM Colors WHERE Color_Id = {colorId}";
        //				try
        //				{
        //					using (SqlCommand cmd = new SqlCommand(deleteMeetQuery, _connection))
        //					{
        //						cmd.ExecuteNonQuery();
        //					}
        //				}
        //				catch (Exception ex)
        //				{
        //					Console.WriteLine(ex.Message);
        //				}
        //			}
        //			else if (opnMode == "Update")
        //			{
        //				opnMode = "Update";
        //				colorIdStr = Request.Query["Color_Id"].ToString();
        //			}
        //		}
        //	}

        //	string selectQuery = "SELECT * FROM Colors";
        //	using (SqlCommand cmd = new SqlCommand(selectQuery, _connection))
        //	{
        //		SqlDataReader reader = cmd.ExecuteReader();
        //		while (reader.Read())
        //		{
        //			ColorModel color = new ColorModel();
        //			color.Color_Id = (int)reader[0];
        //			color.Color_Name = (string)reader[1];
        //			_ColorList.Add(color);
        //		}
        //		reader.Close();
        //	}

        //	ViewBag.ColorList = _ColorList;
        //	ViewBag.status = opnMode;
        //	ViewBag.Color_Id = colorIdStr;


        //	return View(ViewBag);

        //	//if (!Request.Query.IsNullOrEmpty())
        //	//{
        //	//	opnMode = Request.Query["opnMode"];
        //	//	int colorId = int.Parse(Request.Query["Color_Id"].ToString());
        //	//	if (opnMode == "Delete")
        //	//	{
        //	//		string deleteMeetQuery = $"DELETE FROM MeetingInformationTable WHERE MeetingId = {colorId}";
        //	//		try
        //	//		{
        //	//			using (SqlCommand cmd = new SqlCommand(deleteMeetQuery, _connection))
        //	//			{
        //	//				cmd.ExecuteNonQuery();
        //	//			}
        //	//		}
        //	//		catch (Exception ex)
        //	//		{
        //	//			Console.WriteLine(ex.Message);
        //	//		}
        //	//	}
        //	//	else if (opnMode == "Update")
        //	//	{
        //	//		opnMode = "Update";
        //	//		colorIdStr = Request.Query["Color_Id"].ToString();

        //	//	}
        //	//}


        //}
        //[HttpPost]
        //public IActionResult ColorIndex(ColorModel color)
        //{
        //	Connection();
        //	string updateQuery = $"UPDATE Colors SET Color_Name = '{color.Color_Name}' WHERE Color_Id = {color.Color_Id}" ;

        //	try
        //	{
        //		using (SqlCommand cmd = new SqlCommand(updateQuery, _connection))
        //		{
        //			var result = cmd.ExecuteNonQuery();
        //		}
        //	}
        //	catch (Exception ex)
        //	{
        //		Console.WriteLine(ex.Message);
        //	}

        //	return RedirectToAction("ColorIndex", "Color");

        //}
        //      public IActionResult CreateColor()
        //      {
        //          return View();
        //      }
        //      [HttpPost]
        //public IActionResult CreateColor(ColorModel color)
        //{
        //	Connection();

        //	color.Color_Name = Request.Form["Color_Name"];

        //	string addColorQuery = $"INSERT INTO Colors VALUES('{color.Color_Name}')";

        //	try
        //	{
        //		using (SqlCommand cmd = new SqlCommand(addColorQuery, _connection))
        //		{
        //			cmd.ExecuteNonQuery();
        //		}
        //	}
        //	catch (Exception ex)
        //	{
        //		Console.WriteLine(ex.Message);
        //	}

        //	return RedirectToAction("ColorIndex");
        //}
    }
}
