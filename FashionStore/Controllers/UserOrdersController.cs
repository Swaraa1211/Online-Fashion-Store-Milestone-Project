using FashionStore.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using NuGet.Protocol;
using Razorpay.Api;
using System.Text;


namespace FashionStore.Controllers
{
    [Authorize]
    public class UserOrdersController : Controller
    {
        IConfiguration _configuration;
        SqlConnection _connection;

        public UserOrdersController(IConfiguration configuration)
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
        public IActionResult UserOrderIndex()
        {
            Connection();
            string? userEmail = User.Identity?.Name;

            string query = $"Select * from Orders Where User_Email = '{userEmail}'";


            using (SqlCommand cmd = new SqlCommand(query, _connection))
            {
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    OrderModel orders = new OrderModel();
                    orders.Order_Id = (int)reader["Order_Id"];
                    orders.Order_Date = (DateTime)reader["Order_Date"];
                    orders.Total_amount = (int)reader["Total_amount"];
                    orders.User_Email = (string)reader["User_Email"];
                    orders.Paid = (string)reader["Is_Paid"];

                    _ordersList.Add(orders);

                }
            }

            string queryItem = $"Select * from OrderItem Where User_Email = '{userEmail}'";
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

        public FileResult Download(IFormCollection orderForm)
        {
            Connection();

            int orderId = Convert.ToInt32(orderForm["OrderId"]);
            DateTime orderDate = Convert.ToDateTime(orderForm["OrderDate"]);
            string? totalAmount = orderForm["TotalAmount"];
            string? paid = orderForm["Paid"];
            string? userEmail = orderForm["UserEmail"];

            List<OrderItemModel> _ordersItemListD = new List<OrderItemModel>();

            string query = $"Select * from OrderItem where Order_Id = {orderId}";

            using (SqlCommand cmd = new SqlCommand(query, _connection))
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

                    _ordersItemListD.Add(orderItems);
                }
            }

            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"Online Fashion Store!!");

            stringBuilder.AppendLine($"Order ID: {orderId}");
            stringBuilder.AppendLine($"Order Date: {orderDate}");
            stringBuilder.AppendLine($"Total Amount: {totalAmount}");
            stringBuilder.AppendLine($"Payment Status: {paid}");
            stringBuilder.AppendLine($"Email: {userEmail}");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("Order Item ID   Order ID   Product Name   Color   Size   Quantity   Price");
            stringBuilder.AppendLine("===========================================================================");

            foreach (var orderItem in _ordersItemListD)
            {
                stringBuilder.AppendLine($"{orderItem.OrderItem_Id,-14}{orderItem.Order_Id,-11}{orderItem.Product_Name,-15}{orderItem.Color,-8}{orderItem.Size,-7}{orderItem.Quantity,-10}{orderItem.Price,-8}");
            }

            stringBuilder.AppendLine($"Thank You & Visit Again!!");


            byte[] fileBytes = Encoding.ASCII.GetBytes(stringBuilder.ToString());

            return File(fileBytes, "text/plain", $"{orderId}.txt");

                    
           
        }
    }
}