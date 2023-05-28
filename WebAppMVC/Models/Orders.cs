

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WebAppMVC.Models
{
    public class Orders
    {
        public int Id { get; set; }

        public DateTime CreateDate { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public double TotalCost { get; set; }

        public List<OrderDish> OrderDishes { get; set; }
    }
}
