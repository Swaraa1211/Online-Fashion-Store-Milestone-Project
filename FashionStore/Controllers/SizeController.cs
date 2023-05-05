using FashionStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace FashionStore.Controllers
{
    [Authorize]
    public class SizeController : Controller
    {
        IConfiguration _configuration;
        SqlConnection _connection;

        public string operationMode = "";

        public SizeController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }
        private List<SizeModel> _SizeList = new List<SizeModel>();


        public void Connection()
        {
            string? conn = _configuration.GetConnectionString("DefaultConnection");
            _connection = new SqlConnection(conn);
            _connection.Open();
        }
        [HttpGet]
        public IActionResult SizeIndex()
        {
            Connection();
            string? opnMode = "Normal";
            string sizeIdStr = "";

            if (Request.Query.ContainsKey("opnMode") && Request.Query.ContainsKey("Size_Id"))
            {
                opnMode = Request.Query["opnMode"];
                if (int.TryParse(Request.Query["Size_Id"], out int sizeId))
                {
                    if (opnMode == "Delete")
                    {
                        string deleteMeetQuery = $"DELETE FROM Size WHERE Size_Id = {sizeId}";
                        try
                        {
                            using (SqlCommand cmd = new SqlCommand(deleteMeetQuery, _connection))
                            {
                                cmd.ExecuteNonQuery();
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    else if (opnMode == "Update")
                    {
                        opnMode = "Update";
                        sizeIdStr = Request.Query["Size_Id"].ToString();
                    }
                }
            }

            string selectQuery = "SELECT * FROM Size";
            using (SqlCommand cmd = new SqlCommand(selectQuery, _connection))
            {
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    SizeModel size = new SizeModel();
                    size.Size_Id = (int)reader[0];
                    size.Size_Name = (string)reader[1];
                    _SizeList.Add(size);
                }
                reader.Close();
            }

            ViewBag.SizeList = _SizeList;
            ViewBag.status = opnMode;
            ViewBag.Size_Id = sizeIdStr;


            return View(ViewBag);



        }
        [HttpPost]
        public IActionResult SizeIndex(SizeModel size)
        {
            Connection();
            string updateQuery = $"UPDATE Size SET Size_Name = '{size.Size_Name}' WHERE Size_Id = {size.Size_Id}";

            try
            {
                using (SqlCommand cmd = new SqlCommand(updateQuery, _connection))
                {
                    var result = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return RedirectToAction("SizeIndex", "Size");

        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(SizeModel size)
        {
            Connection();

            size.Size_Name = Request.Form["Size_Name"];

            string addScheduleQuery = $"INSERT INTO Size VALUES('{size.Size_Name}')";

            try
            {
                using (SqlCommand cmd = new SqlCommand(addScheduleQuery, _connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return RedirectToAction("SizeIndex");
        }
    }
}
