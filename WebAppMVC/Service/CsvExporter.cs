using Microsoft.AspNetCore.Mvc;
using System.Text;
using WebAppMVC.Models;

namespace WebAppMVC.Service
{
    public class CsvExporter : IExporter
    {
        public MemoryStream exportDishes(List<Dish> dishes)
        {
            var csv = new StringBuilder();
            csv.AppendLine("Name,Price,Stock, TotalOrderd");

            foreach (var dish in dishes)
            {
                csv.AppendLine($"{dish.Name},{dish.Price},{dish.Stock}, {dish.OrderDishes.Where(x => x.DishId == dish.Id).Sum(od => od.Quantity)}");
            }
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream, Encoding.UTF8);
            writer.Write(csv);
            writer.Flush();
            stream.Position = 0;

            return stream;
        }

        public MemoryStream exportOrders(List<Orders> orders)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream, Encoding.UTF8);
            writer.WriteLine("Order ID,Order Date,Order Status,Total Cost,Dishes");

            foreach (var order in orders)
            {
                writer.WriteLine($"{order.Id},{order.CreateDate},{order.OrderStatus},{order.TotalCost},{string.Join(";", order.OrderDishes.Select(od => od.Dish != null ? $"{od.Dish.Name} x {od.Quantity}" : "null"))}");
            }

            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
