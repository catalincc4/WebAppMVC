using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAppMVC.Models;
using WebAppMVC.Repositories;
using WebAppMVC.Service;

namespace WebAppMVC.APIControllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderAPIController : ControllerBase
    {
        private readonly OrderService _orderService;
        public OrderAPIController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("{startDate}/{endDate}")]
        public IEnumerable<OrderAPI> GetOrders(DateTime startDate, DateTime endDate)
        {
            if (_orderService == null)
            {
                return null;
            }
            return _orderService.GetOrdersBetweenDates(startDate, endDate);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddOrder([FromBody] OrderAPI orderAPI)
        {
            if (_orderService == null)
            {
                return BadRequest("Order service is not available.");
            }

            _orderService.Create(_orderService.createOrderAPI(orderAPI));
            await _orderService._unitOfWork.Complete();

            return Ok("Order added successfully.");
        }

        [HttpGet("dish/{startDate}/{endDate}")]
        public IEnumerable<DishAPI> GetMostOderedDishes(DateTime startDate, DateTime endDate)
        {
            if (_orderService == null)
            {
                return null;
            }
            return _orderService.GetMostOrderedDishesBetweenDates(startDate, endDate);
        }
    }

}
