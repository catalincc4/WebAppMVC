
using Microsoft.EntityFrameworkCore;
using WebAppMVC.Models;
using WebAppMVC.Repositories;

namespace WebAppMVC.Service
{
    public class OrderService
    {
        public readonly UnitOfWork _unitOfWork;
        public static List<OrderDish> orderDishes = new List<OrderDish>();
        public static OrderView orderView = new OrderView();

        public OrderService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
      
        public async void Create(Orders order)
        {
            await _unitOfWork.Orders.Add(order);
        }
        public async Task<IEnumerable<Orders>> GetAll()
        {
            return await _unitOfWork.Orders.GetAll();
        }

        public async Task<List<Orders>> GetAlls()
        {
            return await _unitOfWork.Orders.GetAlls();
        }
        public async Task<Orders> Get(int id)
        {
            return await _unitOfWork.Orders.Get(id);
        }

        public async void Update(Orders order)
        {
            _unitOfWork.Orders.Update(order);
        }

        public async void Delete(Orders order)
        {
             _unitOfWork.Orders.Delete(order);
        }
   
        public bool isDuplicate(int id)
        {
            
            return (_unitOfWork.Orders.Get(id) == null) ? false : true;
                
        }
        public async Task<Orders> GetWithDishes(int id)
        {
            return await _unitOfWork.Orders.GetWwithDishes(id);
        }

        public float computeTotalCost(List<OrderDish> orderDishe)
        {
            float totalCost = 0;
            foreach (OrderDish dish in orderDishe)
            {
                totalCost += (float)dish.Dish.Price * dish.Quantity;
            }

            return totalCost;
        }

        public DbSet<Orders> GetOrders()
        {
            return _unitOfWork.Orders.GetOrders();
        }

        public void addOrderDish(int dishId, Dish dish)
        {
            if (orderDishes.Find(x => x.DishId == dishId) == null)
            {
                orderDishes.Add(new OrderDish()
                {
                    DishId = dishId,
                    Quantity = 1,
                    Dish = dish
                });
            }
            else
            {
                OrderDish orderDish = orderDishes.FirstOrDefault(x => x.DishId == dishId);
                orderDish.Quantity += 1;
            }
        }

        public void removeOrderDish(int dishId)
        {
            OrderDish orderDish = orderDishes.FirstOrDefault(x => x.DishId == dishId);
            if (orderDish.Quantity == 1)
            {
                orderDishes.Remove(orderDish);

            }
            else
            {
                orderDish.Quantity -= 1;

            }
        }
        public void createNewOrderView(List<Dish> dishes)
        {
            orderView = new OrderView()
            {
                Order = new Orders()
                {
                    CreateDate = DateTime.Now,
                    TotalCost = computeTotalCost(orderDishes),
                    OrderStatus = OrderStatus.InProgress,
                    OrderDishes = orderDishes
                },
                Dishes = dishes
            };
        }


        public MemoryStream export(DateTime startDate, DateTime endDate, string format)
        {
            var orders = GetAlls().Result.ToList()
           .Where(o => o.CreateDate >= startDate && o.CreateDate <= endDate)
           .OrderByDescending(o => o.OrderDishes.Sum(od => od.Quantity)).ToList();
            IExporter exporter = FactoryExport.Create(format);
            return exporter.exportOrders(orders);

        }

        public List<OrderAPI> GetOrdersBetweenDates(DateTime startDate, DateTime endDate)
        {
            var orders = GetAlls().Result.ToList()
            .Where(o => o.CreateDate >= startDate && o.CreateDate <= endDate).ToList();

            var ordersAPI  = new List<OrderAPI>();
            foreach (var o in orders)
            {
                var dishes = o.OrderDishes.Select(od => od.Dish).ToList();
                var apiDishes =new List<DishAPI>();
                foreach(Dish d in dishes)
                {
                    apiDishes.Add(new DishAPI()
                    {
                        Id = d.Id,
                        Name = d.Name,
                        Price = d.Price,
                        Stock = d.Stock
                    });
                    
                }
                ordersAPI.Add(new OrderAPI()
                {
                    Id = o.Id,
                    CreateDate = o.CreateDate,
                    TotalCost = o.TotalCost,
                    OrderStatus = o.OrderStatus,
                    Dishes = apiDishes
                }) ;
            }
            return ordersAPI;
        }

        public Orders createOrderAPI(OrderAPI orderAPI)
        {
            var orderDishes = new List<OrderDish>();
            foreach(OrderDishAPI orderDishAPI in orderAPI.OrderDishAPIs)
            {
                orderDishes.Add(new OrderDish()
                {
                    DishId = orderDishAPI.DishId,
                    Quantity = orderDishAPI.Quantity
                });
            }
            var order = new Orders()
            {
                CreateDate = DateTime.Now,
                OrderStatus = orderAPI.OrderStatus,
                TotalCost = orderAPI.TotalCost,
                OrderDishes = orderDishes
            };
            return order;
            
        }

        public List<DishAPI> GetMostOrderedDishesBetweenDates(DateTime startDate, DateTime endDate)
        {
            List<DishAPI> dishAPIList = new List<DishAPI>();
            var orders = GetAlls().Result.ToList()
                .Where(o => o.CreateDate >= startDate && o.CreateDate <= endDate);
            foreach(Orders o in orders)
            {
                foreach(OrderDish od in o.OrderDishes)
                {
                    DishAPI d = dishAPIList.FirstOrDefault(d => d.Id == od.DishId);
                    if (d == null) {
                        dishAPIList.Add(new DishAPI()
                        {
                            Id = od.DishId,
                            Name = od.Dish.Name,
                            Price = od.Dish.Price,
                            Stock = od.Quantity
                        }); }
                    else
                    {
                        d.Stock += od.Quantity;
                    }
                }
            }
            return dishAPIList.OrderByDescending(d => d.Stock).ToList();
        }
    }
}
