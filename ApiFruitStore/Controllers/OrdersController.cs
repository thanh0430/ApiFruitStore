using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiFruitStore.Data;
using ApiFruitStore.Models;
using ApiFruitStore.Services;

namespace ApiFruitStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly FruitStoreContext _context;
        private readonly IVnpayServices _vnpPayment;

        public OrdersController(FruitStoreContext context, IVnpayServices vnpPayment)
        {
            _context = context;
            _vnpPayment = vnpPayment;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Orders>>> GetOrders(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var orders = await _context.Orders
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving orders: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Orders>> GetOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
                return NotFound();

            return order;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, Orders order)
        {
            if (id != order.Id)
                return BadRequest();

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Orders>> CreateOrder(Orders order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
                        var order = await _context.Orders.FindAsync(id);
            if (order == null)
                return NotFound();

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpPost("ProcessPayment")]
        public async Task<IActionResult> ProcessPayment(VnPaymentRequestModel paymentRequest)
        {
            var result = await _vnpPayment.CreatePaymentUrl(HttpContext, paymentRequest);
            if (result == null)
                return Unauthorized();

            return Ok(result);
        }
            private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
