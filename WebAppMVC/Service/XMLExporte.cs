using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Xml.Linq;
using WebAppMVC.Models;

namespace WebAppMVC.Service
{
    public class XMLExporte : IExporter
    {
        public MemoryStream exportDishes(List<Dish> dishes)
        {
            var xml = new XElement("Dishes",
                    from dish in dishes
                    select new XElement("Dish",
                        new XElement("Name", dish.Name),
                        new XElement("Price", dish.Price),
                        new XElement("Stock", dish.Stock),
                        new XElement("TotalOrdred", dish.OrderDishes.Where(x => x.DishId == dish.Id).Sum(od => od.Quantity)))

                );
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream, Encoding.UTF8);
            writer.Write(xml.ToString());
            writer.Flush();
            stream.Position = 0;

            return stream;
        }

        public MemoryStream exportOrders(List<Orders> orders)
        {
            var xmlOrders = new XElement("Orders",
            from order in orders
            select new XElement("Order",
                   new XAttribute("Id", order.Id),
                   new XAttribute("CreateDate", order.CreateDate),
                   new XAttribute("OrderStatus", order.OrderStatus),
                   new XAttribute("TotalCost", order.TotalCost),
                   new XElement("Dishes",
                   from od in order.OrderDishes
            select new XElement("Dish",
                od.Dish != null ? new XAttribute("Name", od.Dish.Name) : null,
                new XAttribute("Quantity", od.Quantity)
            )
        )
    )
);

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream, Encoding.UTF8);
            writer.Write(xmlOrders.ToString());
            writer.Flush();
            stream.Position = 0;

            return stream;
        }

    }
}
