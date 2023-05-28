namespace WebAppMVC.Models
{
    public class OrderAPI
    {
        public int Id { get; set; }

        public DateTime CreateDate { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public double TotalCost { get; set; }

        public List<DishAPI> Dishes { get; set; }
         public List<OrderDishAPI> OrderDishAPIs { get; set; }
    }
}
