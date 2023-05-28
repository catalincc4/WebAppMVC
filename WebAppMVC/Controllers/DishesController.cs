using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCMovie.Data;
using WebAppMVC.Models;
using WebAppMVC.Service;

namespace WebAppMVC.Controllers
{
    public class DishesController : Controller
    {
        private readonly DishService _dishService;

        public DishesController(DishService dishService)
        {
            _dishService = dishService;
        }

        // GET: Dishes
        public async Task<IActionResult> Index()
        {
            return _dishService != null ?
                        View(await _dishService.GetAll()) :
                        Problem("Entity set 'MVCMovieContext.Dish'  is null.");
        }

        // GET: Dishes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _dishService == null)
            {
                return NotFound();
            }

            var dish = await _dishService.Get((int)id);
            if (dish == null)
            {
                return NotFound();
            }

            return View(dish);
        }

        // GET: Dishes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Dishes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Stock")] DishView viewModel)
        {

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var dish = new Dish
            {
                Name = viewModel.Name,
                Price = viewModel.Price,
                Stock = viewModel.Stock
            };

            _dishService.Create(dish);
            await _dishService._unitOfWork.Complete();

            return RedirectToAction(nameof(Index));
        }

        // GET: Dishes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _dishService == null)
            {
                return NotFound();
            }

            var dish = await _dishService.Get((int) id);
            if (dish == null)
            {
                return NotFound();
            }
            return View(dish);
        }

        // POST: Dishes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Stock")] DishView viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var dish = new Dish
            { Id = viewModel.Id,
                Name = viewModel.Name,
                Price = viewModel.Price,
                Stock = viewModel.Stock
            };

         
            try
                {
                    _dishService.Update(dish);
                    await _dishService._unitOfWork.Complete();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DishExists(dish.Id))
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

        // GET: Dishes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _dishService == null)
            {
                return NotFound();
            }

            var dish = await _dishService.Get((int) id);
            if (dish == null)
            {
                return NotFound();
            }

            return View(dish);
        }

        // POST: Dishes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_dishService == null)
            {
                return Problem("Entity set 'MVCMovieContext.Dish'  is null.");
            }
            var dish = await _dishService.Get(id);
            if (dish != null)
            {
                _dishService.Delete(dish);
            }

            await _dishService._unitOfWork.Complete();
            return RedirectToAction(nameof(Index));
        }

        private bool DishExists(int id)
        {
            return _dishService.isDuplicate(id);
        }

        public IActionResult Export(string format)
        {
            return File(_dishService.export(format), "test/" + format,"dishes." + format);

        }

    }
}
