using MVCMovie.Data;
using WebAppMVC.Models;
namespace WebAppMVC.Repository
{
    public class UserRepository : GenericRepository<Users>
    {
        public UserRepository(RestaurantContext context) : base(context)
        {

        }
    }
}
