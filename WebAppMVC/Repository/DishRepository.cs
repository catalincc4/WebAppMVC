using Microsoft.EntityFrameworkCore;
using MVCMovie.Data;
using WebAppMVC.Models;

namespace WebAppMVC.Repository
{
    public class DishRepository : GenericRepository<Dish>
    {
        public DishRepository(RestaurantContext context) : base(context)
        {

        }

        public async Task<List<Dish>> GetWwithOrrderDishes() => await
      _context.Dish.Include(o => o.OrderDishes).ToListAsync();
    

    public void UpdateDishes(List<Dish> dishes)
    {
        dishes.ForEach(d => _context.Dish.Update(d));
    } 
}
}
