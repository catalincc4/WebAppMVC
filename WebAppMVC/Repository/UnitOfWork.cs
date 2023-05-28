using MVCMovie.Data;
using WebAppMVC.Repository;

namespace WebAppMVC.Repositories
{
    public class UnitOfWork : IDisposable
    {
        private readonly RestaurantContext _context;

        public OrderRepository Orders { get; }

        public DishRepository Dishes { get; }
         public UserRepository Users { get; }

        public UnitOfWork(RestaurantContext restaurantContext,OrderRepository order, DishRepository dish, UserRepository users)
        {
            _context = restaurantContext;
            Orders = order;
            Dishes = dish;
            Users = users;
        }
        public Task<int> Complete()
        {
            return _context.SaveChangesAsync();
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }

        }
    }
}
    



       
