namespace WebAppMVC.Models
{
    public class OrderView
    {
        public int Id { get; set; }
        public Orders Order { get; set; }
        public List<Dish> Dishes { get; set; }
    }
}
