using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using MessagePack.Formatters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCMovie.Data;
using WebAppMVC.Models;
using WebAppMVC.Repositories;
using WebAppMVC.Service;

namespace WebAppMVC.Controllers
{
    public class OrdersController : Controller
    {
        private static OrderService _orderService;
        private static DishService _dishService;

        public OrdersController(OrderService orderService, DishService dishService)
        {
            _orderService = orderService;
            _dishService = dishService;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
         
            return _orderService != null ?
                        View(await _orderService.GetAlls()):
                        Problem("Entity set 'RestaurantContext.Orders'  is null.");
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _orderService == null)
            {
                return NotFound();
            }

            var orders = await _orderService.Get((int)id);
            if (orders == null)
            {
                return NotFound();
            }

            return View(orders);
        }
    
        public async Task<IActionResult> AddDish(int dishId)
        {
          
            Dish dish = await _dishService.Get(dishId);
            _orderService.addOrderDish(dishId, dish);
            _dishService.updateStockDish(dishId, -1);
            _orderService.createNewOrderView(DishService.dishesOrder.FindAll(d => d.Stock > 0));
            return View("Create", OrderService.orderView);
        }
        public async Task<IActionResult> RemoveDish(int dishId)
        {
            _orderService.removeOrderDish(dishId);
            _dishService.updateStockDish(dishId, 1);
            _orderService.createNewOrderView(DishService.dishesOrder.FindAll(d => d.Stock > 0));
            return View("Create", OrderService.orderView);
        }

        // GET: Orders/Create
        public async Task<IActionResult> Create()
        {
            OrderService.orderDishes = new List<OrderDish>();
            var d = await _dishService.GetAll();
            DishService.dishesOrder = d.ToList();
            _orderService.createNewOrderView(DishService.dishesOrder.FindAll(d => d.Stock > 0));
            return View(OrderService.orderView);
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Order, Dishes")] OrderView orderView)
        {

            if (OrderService.orderView.Order.OrderDishes.Count() > 0)
            {
                 OrderService.orderView.Order.OrderDishes.ForEach(a => a.Dish = null);
                _orderService.Create(OrderService.orderView.Order);
                await _orderService._unitOfWork.Complete();
                _dishService.UpdateDishes(_dishService.updateDishes(OrderService.orderView));
                await _dishService._unitOfWork.Complete();
            }
     
       
            return View(OrderService.orderView);
     }


        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _orderService == null)
            {
                return NotFound();
            }

            var orders = await _orderService.Get((int)id);
            if (orders == null)
            {
                return NotFound();
            }
            
            
            return View(orders);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CreateDate,OrderStatus,TotalCost")] OrderEdit orders)
        {
            if (id != orders.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var o = new Orders()
                    {
                        Id = orders.Id,
                        OrderStatus = orders.OrderStatus,
                        TotalCost = orders.TotalCost,
                        CreateDate = orders.CreateDate
                    };
                   _orderService.Update(o);
                    await _orderService._unitOfWork.Complete();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrdersExists(orders.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(orders);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _orderService == null)
            {
                return NotFound();
            }

            var orders = await _orderService.Get((int)id);
            if (orders == null)
            {
                return NotFound();
            }

            return View(orders);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_orderService == null)
            {
                return Problem("Entity set 'RestaurantContext.Orders'  is null.");
            }
            var orders = await _orderService.Get((int) id);
            if (orders != null)
            {
                _orderService.Delete(orders);
            }

             await _orderService._unitOfWork.Complete();
            return RedirectToAction(nameof(Index));
        }

        private bool OrdersExists(int id)
        {
            return _orderService.isDuplicate(id);
        }

        [HttpPost]
        public IActionResult Export(DateTime startDate, DateTime endDate, string format)
        {
            return File(_orderService.export(startDate, endDate, format),"test/" + format,"orders"+format);
        }


    }
}
