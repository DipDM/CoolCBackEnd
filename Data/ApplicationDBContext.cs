using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolCBackEnd.Controllers;
using CoolCBackEnd.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CoolCBackEnd.Data
{
    public class ApplicationDBContext : IdentityDbContext<User, ApplicationRole, string>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
            : base(options)
        {

        }

        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<ShippingDetail> ShippingDetails { get; set; }
        public DbSet<Admin> Admins { get; set; }

        public DbSet<ApplicationRole> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed roles here
            modelBuilder.Entity<ApplicationRole>().HasData(
                new ApplicationRole { Id = "admin-role-id", Name = "Admin", NormalizedName = "ADMIN",Description = "Admin role" },
                new ApplicationRole { Id = "user-role-id", Name = "User", NormalizedName = "USER",Description = "Admin role" }
            );

            modelBuilder.Entity<ApplicationRole>()
        .ToTable("AspNetRoles")
        .HasKey(r => r.Id);

          modelBuilder.Entity<ApplicationRole>()
        .Property(r => r.Description)
        .HasColumnType("nvarchar(256)")
        .HasMaxLength(256);


            // Configure relationships
            modelBuilder.Entity<Cart>()
                .HasKey(c => c.CartId);

            modelBuilder.Entity<CartItem>()
                .HasKey(ci => ci.CartItemId);

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Cart)
                .WithMany(c => c.CartItems)
                .HasForeignKey(ci => ci.CartId);

            modelBuilder.Entity<Order>().HasKey(o => o.OrderId);
            modelBuilder.Entity<OrderItem>().HasKey(oi => oi.OrderItemId);
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId);
        }
    }
}