using WebAppMVC.Models;
using WebAppMVC.Repositories;

namespace WebAppMVC.Service
{
    public class DishService
    {

        public readonly UnitOfWork _unitOfWork;
        public static List<Dish> dishesOrder = new List<Dish>();
        public DishService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async void Create(Dish dish)
        {
            await _unitOfWork.Dishes.Add(dish);
        }
        public async Task<IEnumerable<Dish>> GetAll()
        {
            return await _unitOfWork.Dishes.GetAll();
        }

        public async Task<Dish> Get(int id)
        {
            return await _unitOfWork.Dishes.Get(id);
        }

        public async void Update(Dish dish)
        {
            _unitOfWork.Dishes.Update(dish);
        }

        public async void Delete(Dish dish)
        {
            _unitOfWork.Dishes.Delete(dish);
        }

        public bool isDuplicate(int id)
        {

            return (_unitOfWork.Dishes.Get(id) == null) ? false : true;

        }
        public async Task<List<Dish>> GetWwithOrrderDishes()
        {
            return await _unitOfWork.Dishes.GetWwithOrrderDishes();
        }

        public void UpdateDishes(List<Dish> dishes)
        {
            _unitOfWork.Dishes.UpdateDishes(dishes);
        }
        public void updateStockDish(int dishId, int val)
        {
            Dish dish = dishesOrder.FirstOrDefault(d => d.Id == dishId);
            dish.Stock += val;
        }

        public List<Dish> updateDishes(OrderView orderView)
        {
            if (orderView.Order.OrderDishes.Count() > 0)
            {
                var dishToUpdate = dishesOrder.FindAll(d => orderView.Order.OrderDishes.FirstOrDefault(x => x.DishId == d.Id) != null);
                return dishToUpdate;
            }
            return null;
        }

        public MemoryStream export(string format)
        {
            IExporter exporter = FactoryExport.Create(format);
            var dishes = GetWwithOrrderDishes().Result.ToList()
            .OrderByDescending(o => o.OrderDishes.Where(x => x.DishId == o.Id).Sum(od => od.Quantity));

            return exporter.exportDishes(dishes.ToList());
        }

    }
}
