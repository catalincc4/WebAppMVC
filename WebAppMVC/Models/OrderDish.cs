namespace WebAppMVC.Models
{
public class OrderDish
{
        public int Id { get; set; }
    public int OrderId { get; set; }
    public Orders Order { get; set; }
    public int DishId { get; set; }
    public Dish Dish { get; set; }
    public int Quantity { get; set; }
}

}
