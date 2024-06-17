using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestTask1.Models;

namespace TestTask1.Context
{
    public class IncidentsManagContext : DbContext
    {
        public IncidentsManagContext(DbContextOptions<IncidentsManagContext> options)
            : base(options)
        {    
        }

        public DbSet<Incident> Incidents { get; set; }
        public DbSet<Account> Accounts { get; set;}
        public DbSet<Contact> Contacts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Incident>()
                .Property(i => i.Name)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Account>()
                .HasIndex(a => a.Name)
                .IsUnique();
        }
    }
}
