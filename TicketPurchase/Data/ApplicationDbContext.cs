using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TicketPurchase.Models;

namespace TicketPurchase.Data
{
    public class IPN
    {
        [Key]
        [Display(Name="Transaction ID")]
        public string TransactionID { get; set; }
        [Display(Name = "Purchase Time")]
        public DateTime TxTime { get; set; }
        [Display(Name ="First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "sessionID")]
        public string SessionID { get; set; }
        [Display(Name = "Total Tickets")]
        public string TotalTickets { get; set; }
        [Display(Name ="Transaction Amount")]
        public decimal TransactionAmount { get; set; }
        [Display(Name = "Payment Status")]
        public string PaymentStatus { get; set; }
    }
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
        public DbSet<IPN> IPNs { get; set; }
    }
}
