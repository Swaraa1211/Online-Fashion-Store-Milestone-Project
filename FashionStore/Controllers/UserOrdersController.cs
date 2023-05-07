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

            //int orderId = Convert.ToInt32(orderForm["OrderId"]);

            //// Get the order data and order item data from the database
            //string orderData = "";
            //string orderItemsData = "";
            //string connectionString = "Server=5CG7324TYL;Database=FashionDB;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=False";

            //using (SqlConnection connection = new SqlConnection(connectionString))
            //{
            //    connection.Open();

            //    // Retrieve the order data
            //    string orderQuery = "SELECT * FROM Orders WHERE Order_Id = @OrderId";
            //    using (SqlCommand orderCommand = new SqlCommand(orderQuery, connection))
            //    {
            //        orderCommand.Parameters.AddWithValue("@OrderId", orderId);
            //        SqlDataReader orderReader = orderCommand.ExecuteReader();
            //        while (orderReader.Read())
            //        {
            //            orderData += "Order Id: " + orderReader["Order_Id"] + "\n";
            //            orderData += "Order Date: " + orderReader["Order_Date"] + "\n";
            //            orderData += "Total Amount: " + orderReader["Total_Amount"] + "\n";
            //            // Add more data as needed
            //        }
            //        orderReader.Close();
            //    }

            //    // Retrieve the order item data
            //    string orderItemsQuery = "SELECT * FROM OrderItem WHERE Order_Id = @OrderId";
            //    using (SqlCommand orderItemsCommand = new SqlCommand(orderItemsQuery, connection))
            //    {
            //        orderItemsCommand.Parameters.AddWithValue("@OrderId", orderId);
            //        SqlDataReader orderItemsReader = orderItemsCommand.ExecuteReader();

            //        while (orderItemsReader.Read())
            //        {
            //            orderItemsData += "Item Id: " + orderItemsReader["OrderItem_Id"] + "\n";
            //            orderItemsData += "Order Id: " + orderItemsReader["Order_Id"] + "\n";
            //            orderItemsData += "Product Id: " + orderItemsReader["Product_Id"] + "\n";
            //            orderItemsData += "Product Name: " + orderItemsReader["Product_name"] + "\n";
            //            orderItemsData += "Color: " + orderItemsReader["Color"] + "\n";
            //            orderItemsData += "Size: " + orderItemsReader["Size"] + "\n";
            //            orderItemsData += "Quantity: " + orderItemsReader["Quantity"] + "\n";
            //            orderItemsData += "Price: " + orderItemsReader["Price"] + "\n";
            //            // Add more data as needed
            //        }
            //        orderItemsReader.Close();
            //    }
            //}

            //// Create a new PDF document
            //Document document = new Document();

            //    // Create a PDF writer that writes to a memory stream
            //MemoryStream memoryStream = new MemoryStream();
            //PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);

            //    // Open the PDF document
            //document.Open();

            //    // Add the order data to the PDF document
            //Paragraph orderParagraph = new Paragraph(orderData);
            //document.Add(orderParagraph);

            //// Add the order item data to the PDF document

            //// Split the order items into an array
            //string[] orderItems = orderItemsData.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            //// Add a heading for the order items
            //Paragraph heading = new Paragraph("Order Items");
            //heading.Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12f);
            //document.Add(heading);

            //// Create a list to display the order items
            //List orderList = new List(List.UNORDERED);
            //for (int i = 0; i < orderItems.Length; i++)
            //{
            //    orderList.Add(new ListItem(orderItems[i]));
            //}

            //document.Add(orderList);

            ////    PdfPTable orderItemsTable = new PdfPTable(8);
            ////    orderItemsTable.AddCell("Item Id");
            ////orderItemsTable.AddCell("Order Id");
            ////orderItemsTable.AddCell("Product Id");
            ////orderItemsTable.AddCell("Product Name");
            ////orderItemsTable.AddCell("Color");
            ////orderItemsTable.AddCell("Size");
            ////orderItemsTable.AddCell("Quantity");
            ////    orderItemsTable.AddCell("Price");
            ////    string[] orderItems = orderItemsData.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            ////for (int i = 0; i < orderItems.Length; i += 8)
            ////{
            ////    if (i + 7 < orderItems.Length)
            ////    {
            ////        orderItemsTable.AddCell(orderItems[i]);
            ////        orderItemsTable.AddCell(orderItems[i + 1]);
            ////        orderItemsTable.AddCell(orderItems[i + 2]);
            ////        orderItemsTable.AddCell(orderItems[i + 3]);
            ////        orderItemsTable.AddCell(orderItems[i + 4]);
            ////        orderItemsTable.AddCell(orderItems[i + 5]);
            ////        orderItemsTable.AddCell(orderItems[i + 6]);
            ////        orderItemsTable.AddCell(orderItems[i + 7]);
            ////    }
            ////}

            ////document.Add(orderItemsTable);

            //// Close the PDF document
            //document.Close();

            //// Set the response headers to download the PDF file
            //Response.Headers["Content-Disposition"] = $"attachment; filename={orderId}.pdf";
            //Response.Headers["Cache-Control"] = "no-cache, no-store";
            //Response.Headers["Pragma"] = "no-cache";
            //Response.Headers["Expires"] = "0";

            //// Write the PDF file to the response output stream
            //Response.Body.Write(memoryStream.ToArray());


            //// Set the response headers to download the PDF file
            ////Response.ContentType = "application/pdf";
            ////Response.Headers.Add("content-disposition", $"attachment;filename={orderId}.pdf");
            ////Response.Cache.SetCacheability(HttpCacheability.NoCache);

            ////// Write the PDF file to the response output stream
            ////Response.BinaryWrite(memoryStream.ToArray());
            ////Response.End();

            //// Return an empty result
            //return File(memoryStream.ToArray(), "application/pdf");


            //normal txt file
            //Connection();

            //int orderId = Convert.ToInt32(orderForm["OrderId"]);
            //DateTime orderDate = Convert.ToDateTime(orderForm["OrderDate"]);
            //string? totalAmount = orderForm["TotalAmount"];
            //string? userEmail = orderForm["UserEmail"];

            //List<OrderItemModel> _ordersItemListD = new List<OrderItemModel>();

            //string query = $"Select * from OrderItem where Order_Id = {orderId}";

            //using (SqlCommand cmd = new SqlCommand(query, _connection))
            //{
            //    SqlDataReader reader = cmd.ExecuteReader();
            //    while (reader.Read())
            //    {
            //        OrderItemModel orderItems = new OrderItemModel();
            //        orderItems.OrderItem_Id = (int)reader["OrderItem_Id"];
            //        orderItems.Order_Id = (int)reader["Order_Id"];
            //        orderItems.Product_Id = (int)reader["Product_Id"];
            //        orderItems.Product_Name = (string)reader["Product_Name"];
            //        orderItems.Color = (string)reader["Color"];
            //        orderItems.Size = (string)reader["Size"];
            //        orderItems.Quantity = (int)reader["Quantity"];
            //        orderItems.Price = (int)reader["Price"];
            //        orderItems.User_Email = (string)reader["User_Email"];

            //        _ordersItemListD.Add(orderItems);

            //    }
            //}
            //StringBuilder stringBuilder = new StringBuilder();


            //stringBuilder.AppendLine($"Order ID -> {orderId} ");
            //stringBuilder.AppendLine();
            //stringBuilder.AppendLine($"Order Date -> {orderDate} ");
            //stringBuilder.AppendLine();
            //stringBuilder.AppendLine($"Total Amount -> {totalAmount} ");
            //stringBuilder.AppendLine();
            //stringBuilder.AppendLine($"Email -> {userEmail} ");
            //stringBuilder.AppendLine();

            //stringBuilder.AppendLine("Order Item ID, Order ID, Product Name,Color, Size, Quantity, Price");
            //foreach (var orderItem in _ordersItemListD)
            //{
            //    stringBuilder.AppendLine($"{orderItem.OrderItem_Id}, {orderItem.Order_Id}, {orderItem.Product_Name},{orderItem.Color},{orderItem.Size}, {orderItem.Quantity}, {orderItem.Price}");
            //}

            //byte[] fileBytes = Encoding.ASCII.GetBytes(stringBuilder.ToString());

            //return File(fileBytes, "text/plain", $"{orderId}.txt");
        }
    }
}