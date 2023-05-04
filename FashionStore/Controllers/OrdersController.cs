using FashionStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Razorpay.Api;

namespace FashionStore.Controllers
{
    public class OrdersController : Controller
    {
        IConfiguration _configuration;
        SqlConnection _connection;

        public OrdersController(IConfiguration configuration)
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

        List<OrderModel> _ordersList = new List<OrderModel>();
        List<OrderItemModel> _ordersItemList = new List<OrderItemModel>();

        public IActionResult OrderIndex()
        {
            Connection();

            //ProductsModel products = new ProductsModel();
            
            

            string query = $"Select * from Orders";

            //string query = $"SELECT o.Order_Id, o.Order_Date, o.Payment, o.User_Email, oi.Product_name, oi.Quantity, oi.Price, p.Color, p.Size"+
            //    "FROM Orders o"+ 
            //    "INNER JOIN OrderItem oi ON o.Order_Id = oi.Order_Id" + 
            //    "INNER JOIN Products p ON oi.Product_Id = p.Product_Id;";

            using (SqlCommand cmd = new SqlCommand(query, _connection))
            {
                SqlDataReader reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    OrderModel orders = new OrderModel();
                    orders.Order_Id = (int)reader["Order_Id"];
                    orders.Order_Date = (DateTime)reader["Order_Date"];
                    orders.Total_amount = (int)reader["Total_amount"];
                    //orders.Payment = (bool)reader[0];
                    //orders.Product_Name = (string)reader[0];
                    //orders.Quantity = (int)reader[0];
                    //orders.Price = (int)reader[0];
                    //orders.Products.Color = (string)reader[0];
                    //orders.Products.Size = (string)reader[0];
                    orders.User_Email = (string)reader["User_Email"];

                    _ordersList.Add(orders);

                }
            }

            string queryItem = $"Select * from OrderItem";
            using (SqlCommand cmd = new SqlCommand(queryItem, _connection))
            {
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    OrderItemModel orderItems = new OrderItemModel();
                    orderItems.OrderItem_Id = (int)reader["OrderItem_Id"];
                    orderItems.Order_Id = (int)reader["Order_Id"];
                    orderItems.Product_Id = (int)reader["Product_Id"];
                    orderItems.Product_Name = (string)reader["Product_Name"];
                    orderItems.Color = (string)reader["Color"];
                    orderItems.Size = (string)reader["Size"];
                    orderItems.Quantity = (int)reader["Quantity"];
                    orderItems.Price = (int)reader["Price"];
                    orderItems.User_Email = (string)reader["User_Email"];

                    _ordersItemList.Add(orderItems);

                }
            }

            ViewBag.OrdersList = _ordersList;
            ViewBag.OrdersItemList = _ordersItemList;

            return View(ViewBag);
        }
    }
}
