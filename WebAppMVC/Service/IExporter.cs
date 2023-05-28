using Microsoft.AspNetCore.Mvc;
using WebAppMVC.Models;

namespace WebAppMVC.Service
{
    public interface IExporter
    {
        public MemoryStream exportOrders(List<Orders> orders);
        public MemoryStream exportDishes(List<Dish> dishes);
    }
}
