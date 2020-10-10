using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Data.Model;
using Data.ViewModel;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Client.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<ToDoList> ToDoLists { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Transaction>Transactions { get; set; }
        public DbSet<Token> Token { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
       
        public ApplicationDbContext() { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
                   .Build();
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
    }
}
