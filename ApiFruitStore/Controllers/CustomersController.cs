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
        public async Task<ActionResult<SignIn>> PostTaiKhoan(SignIn SignIn)
        {
            try
            {
                // Kiểm tra xem email có đúng định dạng không
                if (!IsValidEmail(SignIn.Email))
                {
                    return BadRequest("Email chưa đúng định dạng.");
                }
                Customers taikhoan = new Customers();
                taikhoan.Email = SignIn.Email;
                taikhoan.Password = SignIn.Password;
                taikhoan.Role = "user";
                // Kiểm tra độ dài của password
                if (string.IsNullOrEmpty(taikhoan.Password) || taikhoan.Password.Length < 8)
                {
                    return BadRequest("Password ít nhất phải 8 kí tự.");
                }

                var existingCustomer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == taikhoan.Email);
                if (existingCustomer != null)
                {
                    return Conflict("Email đã tồn tại.");
                }

                _context.Customers.Add(taikhoan);
                await _context.SaveChangesAsync();

                // Sửa lại tên hành động thành "GetTaiKhoan" ở đây
                return CreatedAtAction(nameof(GetCustomers), new { id = taikhoan.Id }, taikhoan);
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
                Customers customers = new Customers();
                customers.Email = signIn.Email;
                customers.Password = signIn.Password;
                customers.Role = "user";
                var user = await _context.Customers.SingleOrDefaultAsync(p => p.Email == signIn.Email && p.Role == "user");


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
        [HttpPost("admindk")]
        public async Task<ActionResult<Customers>> Postadmindk(Customers taikhoan)
        {
            try
            {
                // Kiểm tra xem email có đúng định dạng không
                if (!IsValidEmail(taikhoan.Email))
                {
                    return BadRequest("Email chưa đúng định dạng.");
                }

                // Kiểm tra độ dài của password
                if (string.IsNullOrEmpty(taikhoan.Password) || taikhoan.Password.Length < 8)
                {
                    return BadRequest("Password ít nhất phải 8 kí tự.");
                }

                var existingCustomer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == taikhoan.Email);
                if (existingCustomer != null)
                {
                    return Conflict("Email đã tồn tại.");
                }

                _context.Customers.Add(taikhoan);
                await _context.SaveChangesAsync();

                // Sửa lại tên hành động thành "GetTaiKhoan" ở đây
                return CreatedAtAction(nameof(GetCustomers), new { id = taikhoan.Id }, taikhoan);
            }
            catch (Exception ex)
            {
                // Xử lý bất kỳ lỗi nào xảy ra
                return StatusCode(500, $"Lỗi: {ex.Message}");
            }
        }
        [HttpPost("SignInadmin")]
        public async Task<IActionResult> SignInadmin(SignIn taiKhoanadmin)
        {
            try
            {
                Customers taiKhoan = new Customers();
                taiKhoan.Role = "admin";
                taiKhoan.Email = taiKhoanadmin.Email;
                taiKhoan.Password = taiKhoanadmin.Password;
                var user = await _context.Customers.SingleOrDefaultAsync(p => p.Email == taiKhoan.Email && p.Role == "admin");

                if (user == null || user.Password != taiKhoan.Password)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "Bạn đã nhập sai tài khoản hoặc mật khẩu"
                    });
                }

                var result = await AccountRepo.SignInAsync(taiKhoanadmin);

                if (string.IsNullOrEmpty(result))
                {
                    return Unauthorized();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {

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
