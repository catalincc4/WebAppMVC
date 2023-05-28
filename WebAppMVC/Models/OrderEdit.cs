namespace WebAppMVC.Models
{
    public class OrderEdit
    {
        public int Id { get; set; }

        public DateTime CreateDate { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public double TotalCost { get; set; }
    }
}
