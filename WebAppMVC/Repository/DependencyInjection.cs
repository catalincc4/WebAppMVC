using Microsoft.EntityFrameworkCore;
using MVCMovie.Data;
using WebAppMVC.Repository;
using WebAppMVC.Service;

namespace WebAppMVC.Repositories
{ 
    public static class DependencyInjection
    {
            public static IServiceCollection AddRepository(this IServiceCollection services,String c)
            {
            services.AddTransient<OrderRepository>();
            services.AddTransient<OrderService>();
            services.AddTransient<DishRepository>();
            services.AddTransient<DishService>();
            services.AddTransient<UserRepository>();
            services.AddTransient<UserService>();
            services.AddTransient<UnitOfWork>();
            

            services.AddDbContext<RestaurantContext>(opt => opt.UseSqlServer(c));
                return services;
            }
        }
}

