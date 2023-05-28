using Azure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MVCMovie.Data;
using MVCMovie.Data;
using System;
using System.Linq;
using WebAppMVC.Models;
using static NuGet.Packaging.PackagingConstants;

namespace MvcMovie.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new RestaurantContext(
                serviceProvider.GetRequiredService<DbContextOptions<RestaurantContext>>()))
            {
                // Look for any dishes.
                if (!context.Dish.Any())
                {
                    // Add some dishes.
                    var dishes = new List<Dish>
                {
                    new Dish { Name = "Sarmale", Price = 12, Stock = 10 },
                    new Dish { Name = "Piure", Price = 15, Stock = 3 },
                    new Dish { Name = "Carnaciori", Price = 5, Stock = 20 }
                };
                    context.Dish.AddRange(dishes);
                    context.SaveChanges();
                }

                // Look for any orders.
                if (!context.Orders.Any())
                {
                    // Add some orders.
                    var orders = new List<Orders>
                {
                    new Orders
                    {
                        CreateDate = DateTime.Now,
                        TotalCost = 27,
                        OrderStatus = OrderStatus.Pending
                         ,
                        OrderDishes = new List<OrderDish>
                        {
                            new OrderDish { DishId = 1, Quantity = 2 },
                            new OrderDish { DishId = 2, Quantity = 1 }
                        }
                    },
                    new Orders
                    {
                        CreateDate = DateTime.Now.AddDays(-1),
                        TotalCost = 15,
                        OrderStatus = OrderStatus.Completed,
                        OrderDishes = new List<OrderDish>
                        {
                            new OrderDish { DishId = 3, Quantity = 3 }
                        }
                    }
                };
                    context.Orders.AddRange(orders);
                    context.SaveChanges();
                }

 

            }
        }
    }

}