using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TicketPurchase.Data;
using TicketPurchase.Models;
using TicketPurchase.Services;

namespace TicketPurchase.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            ViewData["SessionID"] = HttpContext.Session.Id;
            return View();
        }

        public ActionResult PayPal()
        {
            // Here we create an instance of the handler which extracts
            // data from PayPal's POST so we need to pass it the 'Request'
            // object from the controller.
            Paypal_IPN paypalResponse = new Paypal_IPN("test", Request);

            if (paypalResponse.TXN_ID != null)
            {
                IPN ipn = new IPN();
                ipn.TransactionID = paypalResponse.TXN_ID;
                decimal amount = Convert.ToDecimal(paypalResponse.PaymentGross);
                ipn.TransactionAmount = amount;
                //ipn.buyerEmail = paypalResponse.PayerEmail;
                ipn.TxTime = DateTime.Now;
                ipn.SessionID = paypalResponse.Custom;
                ipn.FirstName = paypalResponse.PayerFirstName;
                ipn.LastName = paypalResponse.PayerLastName;
                ipn.TotalTickets = paypalResponse.Quantity;
                ipn.PaymentStatus = paypalResponse.PaymentStatus;
                
                _context.IPNs.Add(ipn);
                _context.SaveChanges();
            }
            return View();
        }


        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult SeeAllPurchases()
        {
            var ipns = _context.IPNs.OrderByDescending(ipn => ipn.TxTime);
            return View(ipns);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Success()
        {
            return View();
        }

        public IActionResult Cancelled()
        {
            return View();
        }
    }
}
