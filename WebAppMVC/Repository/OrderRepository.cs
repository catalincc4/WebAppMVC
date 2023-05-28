using Microsoft.EntityFrameworkCore;
using MVCMovie.Data;
using WebAppMVC.Models;

namespace WebAppMVC.Repository
{
    public class OrderRepository : GenericRepository<Orders>
    {
        public OrderRepository(RestaurantContext context) : base(context)
        {
        }

        public async Task<List<Orders>> GetAlls() => await 
            _context.Orders.Include(o => o.OrderDishes)
            .ThenInclude(od => od.Dish)
            .ToListAsync();

        public async Task<Orders> GetWwithDishes(int id) => await
           _context.Orders.Include(o => o.OrderDishes)
            .ThenInclude(od => od.Dish).FirstAsync(o => o.Id == id);
        public DbSet<Orders> GetOrders()
        {
            return _context.Orders; ;
        }
    }
}
