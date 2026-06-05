using GymManagement.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.DbContexts
{
    public class GymDbContext : DbContext
    {

        public GymDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            //base.OnModelCreating(modelBuilder);
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{

        //    optionsBuilder.UseSqlServer();

        //    //base.OnConfiguring(optionsBuilder);
        //}

        public DbSet<Plan> plans { get; set; }
        public DbSet<Member> members { get; set; }
        public DbSet<Trainer> trainers { get; set; }
        public DbSet<HealthRecord> healthRecords { get; set; }
        public DbSet<Booking> bookings { get; set; }
        public DbSet<Membership> memberships { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<Session> sessions { get; set; }

    }
}
