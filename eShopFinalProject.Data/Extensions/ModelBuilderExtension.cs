using eShopFinalProject.Data.Entities;
using eShopFinalProject.Data.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopFinalProject.Data.Extensions
{
    public static class ModelBuilderExtension
    {

        public static void Seed(this ModelBuilder modelBuilder)
        {
            var roleAdminId = new Guid("8D04DCE2-969A-435D-BBA4-DF3F325983DC");
            var roleUserId = new Guid("870c9cb7-e482-4204-9cc0-e69347b043cc");
            var roleSellerId = new Guid("200d51fd-eae5-4951-9734-f4538c85947d");
            var userId = new Guid("69BD714F-9576-45BA-B5B7-F00649BE00DE");

            // USER & ROLE

            modelBuilder.Entity<AppRole>().HasData(
                new AppRole
                {
                    Id = roleAdminId,
                    Name = "admin",
                    NormalizedName = "ADMIN",
                    Description = "Administrator role"
                },
                new AppRole
                {
                    Id = roleSellerId,
                    Name = "seller",
                    NormalizedName = "SELLER",
                    Description = "Seller role"
                },
                new AppRole
                {
                    Id = roleUserId,
                    Name = "user",
                    NormalizedName = "USER",
                    Description = "User role"
                }
            );

            var hasher = new PasswordHasher<AppUser>();
            modelBuilder.Entity<AppUser>().HasData(new AppUser
            {
                Id = userId,
                UserName = "admin@gmail.com",
                NormalizedUserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                NormalizedEmail = "admin@gmail.com",
                Address = "Test Address",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "Abcd1234$"),
                SecurityStamp = string.Empty,
                FullName = "Tuan Nguyen",
                PhoneNumber = "123456",
                Avatar = "TestURL",
            });

            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
            {
                RoleId = roleAdminId,
                UserId = userId
            });

            //
            modelBuilder.Entity<Category>().HasData(
                new Category()
                {
                    Id = 1,
                    Title = "Seed category 1"
                },
                new Category()
                {
                    Id = 2,
                    Title = "Seed category 2"
                });

            modelBuilder.Entity<Color>().HasData(
                new Color()
                {
                    Id = 1,
                    Title = "Red"
                },
                new Color()
                {
                    Id = 2,
                    Title = "Blue"
                });

            modelBuilder.Entity<Brand>().HasData(
                new Brand()
                {
                    Id = 1,
                    Title = "Seed brand 1"
                },
                new Brand()
                {
                    Id = 2,
                    Title = "Seed brand 2"
                });

            modelBuilder.Entity<Coupon>().HasData(
                new Coupon()
                {
                    Id = 1,
                    Name = "BLACKFRIDAY",
                    Expiry = new DateTime(2023, 01, 31),
                    Discount = 10
                },
                new Coupon()
                {
                    Id = 2,
                    Name = "SUMMER",
                    Expiry = new DateTime(2023, 01, 31),
                    Discount = 12
                });

            modelBuilder.Entity<Blog>().HasData(
                new Blog()
                {
                    Id = 1,
                    Title = "This is seed blog 1",
                    Description = "This is seed blog description",
                    NumView = 8,
                    UserId = userId,
                    CategoryId = 1
                },
                new Blog()
                {
                    Id = 2,
                    Title = "This is seed blog 2",
                    Description = "This is seed blog description",
                    NumView = 20,
                    UserId = userId,
                    CategoryId = 2
                });

            modelBuilder.Entity<Order>().HasData(
                new Order()
                {
                    Id = 1,
                    UserId = userId,
                    ShipAddress = "123 To Ky",
                    ShipEmail = "test@gmail.com",
                    ShipPhoneNumber = "12345678",
                    ShipName = "TuanNguyen",
                    orderStatus = OrderStatus.NotProcessed
                },
                new Order()
                {
                    Id = 2,
                    UserId = userId,
                    ShipAddress = "123 To Ky",
                    ShipEmail = "test@gmail.com",
                    ShipPhoneNumber = "12345678",
                    ShipName = "TuanNguyen",
                    orderStatus = OrderStatus.NotProcessed
                });

            modelBuilder.Entity<Product>().HasData(
               new Product()
               {
                   Id = 1,
                   Title = "Seed Product 1",
                   Slug = "seed-product-1",
                   Description = "This is seed data",
                   Price = 10,
                   Quantity = 20,
                   Sold = 0,
                   TotalRating = 1,
                   CategoryId = 1,
                   BrandId = 1
               },
               new Product()
               {
                   Id = 2,
                   Title = "Seed Product 2",
                   Slug = "seed-product-2",
                   Description = "This is seed data",
                   Price = 10,
                   Quantity = 20,
                   Sold = 0,
                   TotalRating = 4,
                   CategoryId = 2,
                   BrandId = 2
               });

            modelBuilder.Entity<ProductInColor>().HasData(
                new ProductInColor()
                {
                    ProductId = 1,
                    ColorId = 1
                },
                new ProductInColor()
                {
                    ProductId = 2,
                    ColorId = 2
                });

            modelBuilder.Entity<ProductInOrder>().HasData(
                new ProductInOrder()
                {
                    ProductId = 1,
                    OrderId = 1
                },
                new ProductInOrder()
                {
                    ProductId = 2,
                    OrderId = 2
                });

            modelBuilder.Entity<ProductRating>().HasData(
                new ProductRating()
                {
                    ProductId = 1,
                    UserId = userId,
                    Star = 5,
                    Comment = "Great"
                },
                new ProductRating()
                {
                    ProductId = 2,
                    UserId = userId,
                    Star = 4,
                    Comment = "Nice"
                });


        }
    }
}
