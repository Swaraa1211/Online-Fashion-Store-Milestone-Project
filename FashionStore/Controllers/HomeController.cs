using FashionStore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace FashionStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(GmailModel model)
        {
            model.Email = Request.Form["Email"];
            //model.Subject = Request.Form["Subject"];
            model.Body = Request.Form["Body"];

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Console.WriteLine("Email " + model.Email);
            Console.WriteLine("Subject " + model.Subject);
            Console.WriteLine("Body " + model.Body);

            // Send the email
            string to = "sai.swaroopa2001@gmail.com";
            string from = "deeshee1211@gmail.com";
                MailMessage message = new MailMessage(from, to);

            string mailBody = model.Email + "\n" + model.Body;
                //message.To.Add("sai.swaroopa2001@gmail.com");
            message.Subject = "Comments / Issue from " + model.Email;
                //message.From = new MailAddress(model.Email);
            message.Body = mailBody;

                message.BodyEncoding = Encoding.UTF8;
                message.IsBodyHtml = false;

                //using (var smtp = new SmtpClient("smtp.gmail.com", 587))
                //{
                //    await smtp.SendMailAsync(message);
                //}

                SmtpClient client = new SmtpClient(); //Gmail smtp    
                client.Host = "smtp.gmail.com";
                client.Port = 587;

            //contactor
            NetworkCredential basicCredential1 = new NetworkCredential("deeshee1211@gmail.com", "mhdxgtxicwwokdkh");
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = basicCredential1;

            try
            {
                client.Send(message);
                TempData["Message"] = "Mail Sent Successfully";

            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error sending contact email");
                ModelState.AddModelError("", "Oops! There was an error submitting your message. Please try again later.");
                return View(model);
            }
            // Redirect to a success page
            return RedirectToAction("Index");
            
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}