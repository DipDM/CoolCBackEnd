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
    public class ApplicationDBContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
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
        public DbSet<ProductSize> ProductSizes { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<CouponUser> CouponUsers { get; set; }
        public DbSet<CouponOrder> CouponOrders { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Rename the UserId property to match the GUID type
            modelBuilder.Entity<User>()
                .Property(u => u.Id)
                .HasColumnName("UserId");

            // Configure foreign keys for User-related entities
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Cart>()
                .HasOne(c => c.User)
                .WithMany(u => u.Carts)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Address>()
                .HasOne(c => c.User)
                .WithMany(u => u.Addresses)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Order>()
                .HasOne(c => c.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId);
            entity.Property(e => e.OrderStatus).IsRequired();
            entity.Property(e => e.PaymentStatus).IsRequired();
            entity.Property(e => e.TotalAmount).IsRequired();
            entity.Property(e => e.UserId).IsRequired();
        });

            // Avoid multiple cascade paths by setting DeleteBehavior to NoAction for ShippingDetail
            modelBuilder.Entity<ShippingDetail>()
                .HasOne(sd => sd.Order)
                .WithMany(o => o.ShippingDetails)
                .HasForeignKey(sd => sd.OrderId)
                .OnDelete(DeleteBehavior.NoAction);

            // Configure relationships for Product, Category, Brand
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Brand)
                .WithMany(b => b.Products)
                .HasForeignKey(p => p.BrandId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            // Coupon Table Configuration
            modelBuilder.Entity<Coupon>(entity =>
            {
                entity.HasKey(e => e.CouponId);

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50);

                // Relationships
                entity.HasMany(c => c.CouponUsers)
                      .WithOne(cu => cu.Coupon)
                      .HasForeignKey(cu => cu.CouponId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(c => c.CouponOrders)
                      .WithOne(co => co.Coupon)
                      .HasForeignKey(co => co.CouponId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // CouponUsers Table Configuration
            modelBuilder.Entity<CouponUser>(entity =>
            {
                entity.HasKey(e => e.CouponUserId);

                entity.HasOne(cu => cu.Coupon)
                    .WithMany(c => c.CouponUsers)
                    .HasForeignKey(cu => cu.CouponId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.OrderId).IsRequired();
                entity.Property(e => e.RedeemedDate).IsRequired();

                // Optional: Add indexes for performance
                entity.HasIndex(cu => cu.UserId);
                entity.HasIndex(cu => cu.OrderId);
            });

            // CouponOrders Table Configuration
            modelBuilder.Entity<CouponOrder>(entity =>
            {
                entity.HasKey(e => e.CouponOrderId);

                entity.HasOne(co => co.Coupon)
                    .WithMany(c => c.CouponOrders)
                    .HasForeignKey(co => co.CouponId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.OrderId).IsRequired();
                entity.Property(e => e.DiscountAmount)
                      .HasColumnType("decimal(18,2)")
                      .IsRequired();

                // Optional: Add indexes for performance
                entity.HasIndex(co => co.OrderId);
            });

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Order)
                .WithMany(o => o.Payments)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Cascade); //Cascade delete if an order is removed
            
            modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId);
            entity.Property(e => e.PaymentMethod).IsRequired();
            entity.Property(e => e.TransactionId).IsRequired();
            entity.Property(e => e.PaymentDate).IsRequired();
            entity.Property(e => e.AmountPaid).IsRequired();
        });

            // Seeding roles with GUID Ids
            List<IdentityRole<Guid>> roles = new List<IdentityRole<Guid>>
        {
            new IdentityRole<Guid>
            {
                Id = Guid.NewGuid(),
                Name = "Admin",
                NormalizedName = "ADMIN"
            },
            new IdentityRole<Guid>
            {
                Id = Guid.NewGuid(),
                Name = "User",
                NormalizedName = "USER"
            }
        };
            modelBuilder.Entity<IdentityRole<Guid>>().HasData(roles);
        }
    }

}