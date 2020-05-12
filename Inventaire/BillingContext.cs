using BillingManagement.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BillingManagement.UI
{
    class BillingContext: DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("Data Source = Billing.db");
        }

        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<ContactInfo> ContactInfos { get; set; }
        public DbSet<Customer> Customers { get; set; }
    }
}
