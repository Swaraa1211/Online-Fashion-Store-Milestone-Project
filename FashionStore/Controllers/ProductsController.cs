﻿using FashionStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Drawing;

namespace FashionStore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductsController : Controller
    {
        IConfiguration _configuration;
        SqlConnection _connection;

        public ProductsController(IConfiguration configuration)
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

        
        public IActionResult ProductIndex()
        {
            Connection();

            string query = "Select * from Products";
            using (SqlCommand sqlCommand = new SqlCommand(query,_connection))
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

            ViewBag.ProductsList = _ProductsList;


            return View(ViewBag);
        }

        List<SelectListItem> colors = new List<SelectListItem>();
        List<SelectListItem> sizes = new List<SelectListItem>();

        public IActionResult CreateProducts()
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
            return View();
        }



        

        [HttpPost]
        public IActionResult CreateProducts(ProductsModel products)
        {
            Connection();

            string color = Request.Form["colors"].ToString();
            string size = Request.Form["sizes"].ToString();

            products.Product_Name = Request.Form["Product_Name"];
            products.Product_Description = Request.Form["Product_Description"];
            products.Product_Image = Request.Form["Product_Image"];
            products.Price = Convert.ToInt32(Request.Form["Price"]);

            string addProductQuery = "INSERT INTO Products VALUES (@ProductName, @ProductDescription, @ProductImage, @Color, @Size, @Price)";

            try
            {
                using (SqlCommand cmd = new SqlCommand(addProductQuery, _connection))
                {
                    cmd.Parameters.AddWithValue("@ProductName", products.Product_Name);
                    cmd.Parameters.AddWithValue("@ProductDescription", products.Product_Description);
                    cmd.Parameters.AddWithValue("@ProductImage", products.Product_Image);
                    cmd.Parameters.AddWithValue("@Color", color);
                    cmd.Parameters.AddWithValue("@Size", size);
                    cmd.Parameters.AddWithValue("@Price", products.Price);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("In Create Product");
            }

            return RedirectToAction("ProductIndex");
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

            

            return products;
        }

        public IActionResult EditProduct(int id)
        {
            Connection();

            colors.Clear();
            sizes.Clear();

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
            return View("EditProduct", GetProducts(id));
        }

        [HttpPost]
        public IActionResult EditProduct(int id, ProductsModel products)
        {
            Connection();
            
            using(SqlCommand cmd = new SqlCommand("Update_Product", _connection))
            {
                
                cmd.CommandType = CommandType.StoredProcedure;

                string color = Request.Form["colors"].ToString();
                string size = Request.Form["sizes"].ToString();

                cmd.Parameters.AddWithValue("@Product_Id", id);
                cmd.Parameters.AddWithValue("@Product_Name", products.Product_Name);
                cmd.Parameters.AddWithValue("@Product_Description", products.Product_Description);
                cmd.Parameters.AddWithValue("@Product_Image", products.Product_Image);
                cmd.Parameters.AddWithValue("@Color", color);
                cmd.Parameters.AddWithValue("@Size", size);
                cmd.Parameters.AddWithValue("@Price", products.Price);

                cmd.ExecuteNonQuery();
            }


            return RedirectToAction("ProductIndex", "Products");
        }

        public IActionResult DeleteProduct(int id)
        {
            return View(GetProducts(id));
        }

        [HttpPost]
        public IActionResult DeleteProduct(int id, ProductsModel product)
        {
            Connection();

            SqlCommand cmd = new SqlCommand("DELETE FROM Products WHERE Product_Id=@Product_Id",
                    _connection);

            cmd.Parameters.AddWithValue("@Product_Id", id);
            cmd.ExecuteNonQuery();

            _connection.Close();
            return RedirectToAction("ProductIndex", "Products");
        }
                            
    }
}
