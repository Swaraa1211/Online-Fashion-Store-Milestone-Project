using FashionStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FashionStore.Controllers
{
    public class UserProductController : Controller
    {
        IConfiguration _configuration;
        SqlConnection _connection;

        public UserProductController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }


        public void Connection()
        {
            string? conn = _configuration.GetConnectionString("DefaultConnection");
            _connection = new SqlConnection(conn);
            _connection.Open();
        }

        List<ProductsModel> _ProductsList = new List<ProductsModel>();

        public IActionResult UserProductIndex()
        {
            Connection();


            string query = "Select * from Products";
            using (SqlCommand sqlCommand = new SqlCommand(query, _connection))
            {
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    ProductsModel products = new ProductsModel();
                    products.Product_Id = (int)reader["Product_Id"];
                    products.Product_Name = (string)reader["Product_Name"];
                    products.Product_Description = (string)reader["Product_Description"];
                    products.Product_Image = (string)reader["Product_Image"];
                    products.Color = (string)reader["Color"];
                    products.Size = (string)reader["Size"];
                    products.Price = (int)reader["Price"];

                    _ProductsList.Add(products);
                }
                reader.Close();
            }

            return View(_ProductsList);

            //Connection();

            //string query = "Select * from Products";
            //using (SqlCommand sqlCommand = new SqlCommand(query, _connection))
            //{
            //    SqlDataReader reader = sqlCommand.ExecuteReader();
            //    while (reader.Read())
            //    {
            //        ProductsModel products = new ProductsModel();
            //        products.Product_Id = (int)reader["Product_Id"];
            //        products.Product_Name = (string)reader["Product_Name"];
            //        products.Product_Description = (string)reader["Product_Description"];
            //        products.Color = (string)reader["Color"];
            //        products.Size = (string)reader["Size"];
            //        products.Price = (decimal)reader["Price"];

            //        _ProductsList.Add(products);

            //    }
            //    reader.Close();
            //}

            //ViewBag.ProductsList = _ProductsList;


            //return View(ViewBag);
        }

        public ProductsModel GetProducts(int id)
        {
            Connection();
            ProductsModel products = new ProductsModel();

            string query = $"Select * from Products where Product_Id = {id}";

            Console.WriteLine("product id" + id);

            SqlCommand cmd = _connection.CreateCommand();

            cmd.CommandText = query;
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                products.Product_Name = (string)reader[1];
                products.Product_Description = (string)reader[2];
                products.Product_Image = (string)reader[3];
                products.Color = (string)reader[4];
                products.Size = (string)reader[5];
                products.Price = (int)reader[6];
            }
            reader.Close();
            _connection.Close();

            //ProductsModel products = new ProductsModel();

            //string query = $"Select Color, Size from Products where Product_Id = {id}";

            //Console.WriteLine("product id" + id);

            //SqlCommand cmd = _connection.CreateCommand();

            //cmd.CommandText = query;
            //SqlDataReader reader = cmd.ExecuteReader();
            //while (reader.Read())
            //{
            //    //products.Color = (string)reader[3];
            //    //products.Size = (string)reader[4];
            //    products.Color = (string)reader["Color"];
            //    products.Size = (string)reader["Size"];
            //}
            //reader.Close();
            //_connection.Close();


            return products;
        }

        List<SelectListItem> colors = new List<SelectListItem>();
        List<SelectListItem> sizes = new List<SelectListItem>();

        public IActionResult EditUserProduct(int id)
        {
            Connection();
            string selectQuery = "SELECT * FROM Colors";
            using (SqlCommand cmd = new SqlCommand(selectQuery, _connection))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        string colorName = (string)reader[1];
                        colors.Add(new SelectListItem(colorName, colorName));
                    }
                }
            }

            ViewBag.ColorList = colors;

            string query = "SELECT * FROM Size";
            using (SqlCommand cmd = new SqlCommand(query, _connection))
            {
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string sizeName = (string)reader[1];
                    sizes.Add(new SelectListItem(sizeName, sizeName));
                }
                reader.Close();
            }

            ViewBag.SizeList = sizes;
            return View(GetProducts(id));
        }

        


        [HttpPost]
        public IActionResult EditUserProduct(int id, ProductsModel products)
        {
            Connection();

            using (SqlCommand cmd = new SqlCommand("Update_Product_User", _connection))
            {
                //SqlCommand cmd = new SqlCommand("UPDATE_DOCUMENT", _connection);
                cmd.CommandType = CommandType.StoredProcedure;

                string color = Request.Form["colors"].ToString();
                string size = Request.Form["sizes"].ToString();

                cmd.Parameters.AddWithValue("@Product_Id", id);
                cmd.Parameters.AddWithValue("@Color", color);
                cmd.Parameters.AddWithValue("@Size", size);

                cmd.ExecuteNonQuery();
            }


            return RedirectToAction("UserProductIndex", "UserProduct");
        }



        public IActionResult AddToCart(int id)
        {
            return View(GetProducts(id));
        }

        [HttpPost]
        public IActionResult AddToCart( string product_name, decimal price, int product_id)
        {
            Console.WriteLine("Inside cart");
            //email = "user1@gmail.com";
            Connection();

            

            int count = 0;

            using (SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM Carts WHERE Product_name = @productName", _connection))
            {
                command.Parameters.AddWithValue("@productName", product_name);
                count = (int)command.ExecuteScalar();
                
            }
            if (count != 0)
            {
                Console.WriteLine("Already there");
                TempData["Message"] = "Already added";

            }
            else
            {
                
                //string updateQuery = "Update Cart Set Quantity =";
                try
                {
                    string insertQuery = "INSERT INTO Carts (Product_Id, Product_name, Quantity, Price) VALUES (@product_id, @product_name, 1, @price)";
                    using (SqlCommand cmd = new SqlCommand(insertQuery, _connection))
                    {
                        cmd.Parameters.AddWithValue("@product_id", product_id);
                        cmd.Parameters.AddWithValue("@product_name", product_name);
                        //cmd.Parameters.AddWithValue("@quantity", quantity);
                        cmd.Parameters.AddWithValue("@price", price);
                        //cmd.Parameters.AddWithValue("@email", email);
                        cmd.ExecuteNonQuery();
                    }
                    TempData["Message"] = "Added to Cart";
                    //ViewBag.Message = "Product added to cart successfully!";
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    //ViewBag.Message = "Error adding product to cart: " + ex.Message;
                }
            }

            // Render the view with the message
            //return View("UserProductIndex");
            return RedirectToAction("UserProductIndex", "UserProduct");
        }

    }
}
