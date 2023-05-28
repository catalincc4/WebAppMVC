using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Models;
using WebAppMVC.Models;

namespace MVCMovie.Data
{
    public class RestaurantContext : DbContext
    {
        public RestaurantContext(DbContextOptions<RestaurantContext> options) : base(options)
        {
        }

        public DbSet<Dish> Dish { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<OrderDish> OrderDish { get; internal set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderDish>()
                .HasKey(od => new { od.OrderId, od.DishId });

            modelBuilder.Entity<OrderDish>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderDishes)
                .HasForeignKey(od => od.OrderId);

            modelBuilder.Entity<OrderDish>()
                .HasOne(od => od.Dish)
                .WithMany(d => d.OrderDishes)
                .HasForeignKey(od => od.DishId);

            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
        }

        public DbSet<Users> Users { get; internal set; }
        public DbSet<IdentityUserClaim<string>> UserClaims { get; set; }



    }

}
