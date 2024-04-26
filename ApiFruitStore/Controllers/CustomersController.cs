using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiFruitStore.Data;
using ApiFruitStore.Repository;
using ApiFruitStore.Models;

namespace ApiFruitStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly FruitStoreContext _context;
        private readonly IAccountRepository AccountRepo;

        public CustomersController(FruitStoreContext context, IAccountRepository repo)
        {
            _context = context;
            AccountRepo = repo;
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customers>>> GetCustomers()
        {
          if (_context.Customers == null)
          {
              return NotFound();
          }
            return await _context.Customers.ToListAsync();
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customers>> GetCustomers(int id)
        {
          if (_context.Customers == null)
          {
              return NotFound();
          }
            var customers = await _context.Customers.FindAsync(id);

            if (customers == null)
            {
                return NotFound();
            }

            return customers;
        }

        // PUT: api/Customers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomers(int id, Customers customers)
        {
            if (id != customers.Id)
            {
                return BadRequest();
            }

            _context.Entry(customers).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomersExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Customers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]

        // Phương thức kiểm tra email có đúng định dạng không
        public async Task<ActionResult<Customers>> PostCustomers(Customers customers)
        {
            try
            {
                // Kiểm tra xem email có đúng định dạng không
                if (!IsValidEmail(customers.Email))
                {
                    return BadRequest("Email chưa đúng định dạng.");
                }

                // Kiểm tra độ dài của password
                if (string.IsNullOrEmpty(customers.Password) || customers.Password.Length < 8)
                {
                    return BadRequest("Password ít nhất phải 8 kí tự.");
                }

                // Kiểm tra xem email đã tồn tại trong cơ sở dữ liệu chưa
                var existingCustomer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == customers.Email);
                if (existingCustomer != null)
                {
                    return Conflict("Email đã tồn tại.");
                }

                if (_context.Customers == null)
                {
                    return Problem("Entity set 'FruitStoreContext.Customers'  is null.");
                }

                _context.Customers.Add(customers);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetCustomers", new { id = customers.Id }, customers);
            }
            catch (Exception ex)
            {
                // Xử lý bất kỳ lỗi nào xảy ra
                return StatusCode(500, $"Lỗi: {ex.Message}");
            }
        }

        // Phương thức kiểm tra email có đúng định dạng không
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn(SignIn signIn)
        {
            try
            {
                var user = await _context.Customers.SingleOrDefaultAsync(p => p.Email == signIn.Email);

                if (user == null || user.Password != signIn.Password)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "Bạn đã nhập sai tài khoản hoặc mật khẩu"
                    });
                }

                var result = await AccountRepo.SignInAsync(signIn);

                if (string.IsNullOrEmpty(result))
                {
                    return Unauthorized();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                // Ghi log lỗi vào hệ thống
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomers(int id)
        {
            if (_context.Customers == null)
            {
                return NotFound();
            }
            var customers = await _context.Customers.FindAsync(id);
            if (customers == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customers);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CustomersExists(int id)
        {
            return (_context.Customers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
