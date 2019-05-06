using HomeCorner2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HomeCorner2.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {

            return View();
        }

        public ActionResult Home()
        {
            var latestHouses = db.Houses.OrderByDescending(house => house.Id).Take(6);

            return View(latestHouses);
        }

        public ActionResult AboutUs()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult ContactUs()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public async Task<ActionResult> Email(FormCollection form)
        {
            var userFullName = form["user-full-name"];
            var userEmail = form["user-email"];
            var userPhone = form["user-phone"];
            var userMessageSubject = form["user-message-subject"];
            var messages = form["user-message"];
            var x = await SendEmail(userFullName, userEmail, userMessageSubject, userPhone);
            if (x == "sent")
                ViewData["esent"] = "Your Message Has Been Sent";
            return RedirectToAction("ContactUs");
        }

        private async Task<string> SendEmail(string name, string email, string messages, string phone)
        {
            var message = new MailMessage();
            message.To.Add(new MailAddress(""));  // replace with receiver's email id  
            message.From = new MailAddress(email);  // replace with sender's email id 
            message.Subject = "Message From " + email;
            message.Body = "Name: " + name + "\nFrom: " + email + "\nPhone: " + phone + "\n" + messages;
            message.IsBodyHtml = true;
            message.ReplyToList.Add(email);
            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = "",  // replace with sender's email id 
                    Password = ""  // replace with password 
                };
                smtp.Credentials = credential;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(message);
                return "sent";
            }
        }
    }
}
