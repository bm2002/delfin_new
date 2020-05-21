using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Web;

namespace qiwi.Models
{
    public class logContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["Logs"].ConnectionString);
                //optionsBuilder.UseSqlServer("Data Source=192.168.0.16;Initial Catalog=Logs;Integrated Security=False;User ID=sa;Password=23#swde23#;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False;");
            }
        }

        public DbSet<RequestDump2> RequestDumps2 { get; set; } 
    }

    public class RequestDump2
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public string ClientIP { get; set; }
        public string Url { get; set; }
        public string Label { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
        [MaxLength(10)]
        public string OrderId { get; set; }
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string Comment { get; set; }
        public int ProviderId { get; set; }
        public bool Success { get; set; }
        public TimeSpan Duration { get; set; }
    }
}