using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCMovie.Data;
using WebAppMVC.Models;
using WebAppMVC.Service;

namespace WebAppMVC.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
              return _userService != null ? 
                          View(await _userService.GetAll()) :
                          Problem("Entity set 'RestaurantContext.Users'  is null.");
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _userService == null)
            {
                return NotFound();
            }

            var users = await _userService.Get((int) id);
            if (users == null)
            {
                return NotFound();
            }

            return View(users);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Username,password")] Users users)
        {
            if (ModelState.IsValid)
            {
                _userService.Create(users);
                await _userService._unitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }
            return View(users);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _userService== null)
            {
                return NotFound();
            }

            var users = await _userService.Get((int) id );
            if (users == null)
            {
                return NotFound();
            }
            return View(users);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Username,password")] Users users)
        {
            if (id != int.Parse(users.Id))
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _userService.Update(users);
                    await _userService._unitOfWork.Complete();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsersExists(int.Parse(users.Id)))
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
            return View(users);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _userService == null)
            {
                return NotFound();
            }

            var users = await _userService.Get((int) id);
            if (users == null)
            {
                return NotFound();
            }

            return View(users);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_userService == null)
            {
                return Problem("Entity set 'RestaurantContext.Users'  is null.");
            }
            var users = await _userService.Get((int) id);
            if (users != null)
            {
                _userService.Delete(users);
            }
            
            await _userService._unitOfWork.Complete();
            return RedirectToAction(nameof(Index));
        }

        private bool UsersExists(int id)
        {
          return _userService.isDuplicate(id);
        }
    }
}
