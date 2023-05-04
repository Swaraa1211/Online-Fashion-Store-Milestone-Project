using FashionStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Razorpay.Api;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace FashionStore.Controllers
{
    public class CartController : Controller
    {
        IConfiguration _configuration;
        SqlConnection _connection;

        public CartController(IConfiguration configuration)
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

        List<CartModel> _cartList = new List<CartModel>();

        public IActionResult CartIndex()
        {
            Connection();
            string? userEmail = User.Identity?.Name;

            Console.WriteLine("Email retrieved:  " + userEmail);

            string query = "Select * from Carts";
            using (SqlCommand sqlCommand = new SqlCommand(query, _connection))
            {
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    CartModel cart = new CartModel();
                    cart.Cart_Id = (int)reader["Cart_Id"];
                    cart.Product_Name = (string)reader["Product_Name"];
                    cart.Quantity = (int)reader["Quantity"];
                    cart.Price = (int)reader["Price"];

                    _cartList.Add(cart);
                }
                reader.Close();
            }

            ViewBag.CartsList = _cartList;


            return View(ViewBag);
        }
        [HttpPost]
        public ActionResult UpdateCartItem(int Cart_Id, int Quantity, int Price)
        {
            Connection();
            
                using (var command = new SqlCommand("UPDATE Carts SET Quantity = @Quantity, Price = @Price WHERE Cart_Id = @Cart_Id", _connection))
                {
                    command.Parameters.AddWithValue("@Cart_Id", Cart_Id);
                    command.Parameters.AddWithValue("@Quantity", Quantity);
                    command.Parameters.AddWithValue("@Price", Price);
                    command.ExecuteNonQuery();
                }
            
            return Json(new { success = true });
        }

        [HttpPost]
        public ActionResult RemoveCartItem(int Cart_Id)
        {
            Connection();
            // Your code to remove the cart item from the database
            Console.WriteLine("In delete " + Cart_Id);
                string query = "DELETE FROM Carts WHERE Cart_Id = @Cart_Id";
                using (SqlCommand command = new SqlCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@Cart_Id", Cart_Id);
                    
                    int result = command.ExecuteNonQuery();
                    
                }
            
            return RedirectToAction("CartIndex");
        }

        public IActionResult Order()
        {
            Connection();

            string query = "Select * from Carts";
            using (SqlCommand sqlCommand = new SqlCommand(query, _connection))
            {
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    CartModel cart = new CartModel();
                    cart.Cart_Id = (int)reader["Cart_Id"];
                    cart.Product_Name = (string)reader["Product_Name"];
                    cart.Quantity = (int)reader["Quantity"];
                    cart.Price = (int)reader["Price"];

                    _cartList.Add(cart);
                }
                reader.Close();
            }

            ViewBag.CartsList = _cartList;


            return View(ViewBag);
        }

        [HttpPost]
        public IActionResult PlaceOrder(string userEmail)
        {
            Connection();

            int totalPrice = Convert.ToInt32(Request.Form["totalPrice"]);
            totalPrice = totalPrice / 2;
            Console.WriteLine("Amount: " + totalPrice);
            //totalPrice = totalPrice / 100;

            // Insert data into Orders table
            string orderQuery = "INSERT INTO Orders (Order_Date,Total_Amount, User_Email) VALUES (@OrderDate,@Total_Amount, @UserEmail)";
            using (SqlCommand sqlCommand = new SqlCommand(orderQuery, _connection))
            {
                sqlCommand.Parameters.AddWithValue("@OrderDate", DateTime.Now);
                sqlCommand.Parameters.AddWithValue("@UserEmail", userEmail);
                sqlCommand.Parameters.AddWithValue("@Total_Amount", totalPrice);
                sqlCommand.ExecuteNonQuery();
            }

            // Get the order ID of the newly inserted order
            int orderId = 0;
            string orderSelectQuery = "SELECT @@IDENTITY AS 'OrderId'";
            using (SqlCommand sqlCommand = new SqlCommand(orderSelectQuery, _connection))
            {
                SqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.Read())
                {
                    orderId = Convert.ToInt32(reader["OrderId"]);
                }
                reader.Close();
            }

            //string color = "";
            //string size = "";

            //string query = $"SELECT p.Product_name, p.Color, p.Size, c.Quantity, c.Price" +
            //                "FROM Products p INNER JOIN Carts c ON p.Product_Id = c.Product_Id;";

            //using (SqlCommand sqlCommand = new SqlCommand(query, _connection))
            //{
            //    SqlDataReader reader = sqlCommand.ExecuteReader();
            //    if (reader.Read())
            //    {
            //        color = (string)reader["Color"];

            //        size = (string)reader["Size"];
            //    }
            //    reader.Close();
            //}

            // Insert data into OrderItem table
            //string orderItemQuery = "INSERT INTO OrderItem (Order_Id, Product_Id, Product_name, Color, Size, Quantity, Price, User_Email) " +
            //                "SELECT @OrderId, Product_Id, Product_Name,  Quantity, Price, @UserEmail FROM Carts"+
            //                "SELECT @Color, @Size, FROM Products";

            string orderItemQuery = "INSERT INTO OrderItem (Order_Id, Product_Id, Product_name, Color, Size, Quantity, Price, User_Email) " +
                        "SELECT @OrderId, C.Product_Id, P.Product_name, P.Color, P.Size, C.Quantity, P.Price, @UserEmail " +
                        "FROM Carts C " +
                        "JOIN Products P ON C.Product_Id = P.Product_Id";



            //string orderItemQuery = "INSERT INTO OrderItem (Order_Id, Product_Id, Product_name,Color,Size, Quantity, Price, User_Email) " +
            //                        "SELECT @OrderId, Product_Id, Product_Name,Color,Size, Quantity, Price, @UserEmail FROM Carts";
            using (SqlCommand sqlCommand = new SqlCommand(orderItemQuery, _connection))
            {
                sqlCommand.Parameters.AddWithValue("@OrderId", orderId);
                sqlCommand.Parameters.AddWithValue("@UserEmail", userEmail);
                //sqlCommand.Parameters.AddWithValue("@Color", color);
                //sqlCommand.Parameters.AddWithValue("@Size", size);

                sqlCommand.ExecuteNonQuery();
            }

            ViewBag.OrderId = orderId;

            Console.WriteLine("in place order: " + orderId);

            return RedirectToAction("Order", new { OrderId = orderId });
        }

        [HttpPost]
        public IActionResult OrderAmountDeleteCart( string amount, int orderId)
        {
            Connection();

            //string query = "INSERT INTO Orders (Total_Amount) " +
            //"VALUES (@Amount)";

            //string query = "Update Orders ";
            //using (SqlCommand cmd = new SqlCommand(query, _connection))
            //{
            //    cmd.Parameters.AddWithValue("@Amount", amount);
            //    cmd.ExecuteNonQuery();
            //}

            //updating is paid or not
            Console.WriteLine("OrderIdfromrazor " + orderId);
            Console.WriteLine("Amount " + amount);

            string updateQuery = $"UPDATE Orders SET Is_Paid = 'Paid' WHERE Order_Id = @OrderId";
            using (SqlCommand sqlCommand = new SqlCommand(updateQuery, _connection))
            {
                sqlCommand.Parameters.AddWithValue("@OrderId", orderId);
                sqlCommand.ExecuteNonQuery();
            }


            //deleting cart table
            string cartDeleteQuery = "DELETE FROM Carts";
            using (SqlCommand cartDeleteCommand = new SqlCommand(cartDeleteQuery, _connection))
            {
                cartDeleteCommand.ExecuteNonQuery();
            }

            //mail
            //To address    
            //string to = "deeshee1211@gmail.com";
            string? to = User.Identity?.Name;
            string from = "sai.swaroopa2001@gmail.com"; //From address    
            MailMessage message = new MailMessage(from, to);
            string mailbody = "Your order has been placed and payment is successfull.\n Your order will be shipped and delivered within 7 working days";
            message.Subject = "Order and Shippment details";
            message.Body = mailbody;

            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = false;

            SmtpClient client = new SmtpClient(); //Gmail smtp    
            client.Host = "smtp.gmail.com";
            client.Port = 587;

            NetworkCredential basicCredential1 = new NetworkCredential("sai.swaroopa2001@gmail.com", "fauquamjjwdyfaiz");
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = basicCredential1;
            try
            {
                client.Send(message);
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            // Redirect to cart page
            return RedirectToAction("CartIndex");
        }


        //public IActionResult PlaceOrder(string userEmail)
        //{
        //    Connection();

        //    string query = "INSERT INTO Orders (Order_Date, User_Email) VALUES (@OrderDate, @UserEmail)";
        //    using (SqlCommand sqlCommand = new SqlCommand(query, _connection))
        //    {
        //        sqlCommand.Parameters.AddWithValue("@OrderDate", DateTime.Now);
        //        sqlCommand.Parameters.AddWithValue("@UserEmail", userEmail);
        //        sqlCommand.ExecuteNonQuery();
        //    }

        //    List<CartModel> _cartList = new List<CartModel>();
        //    query = "Select * from Carts";
        //    using (SqlCommand sqlCommand = new SqlCommand(query, _connection))
        //    {
        //        SqlDataReader reader = sqlCommand.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            CartModel cart = new CartModel();
        //            cart.Cart_Id = (int)reader["Cart_Id"];
        //            cart.Product_Name = (string)reader["Product_Name"];
        //            cart.Quantity = (int)reader["Quantity"];
        //            cart.Price = (int)reader["Price"];

        //            _cartList.Add(cart);
        //        }
        //        reader.Close();
        //    }

        //    ViewBag.CartsList = _cartList;

        //    return View("CartIndex", ViewBag);
        //}

    }
}
