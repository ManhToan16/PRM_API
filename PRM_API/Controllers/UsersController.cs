using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PRM_API.DTO;
using PRM_API.Models;

namespace PRM_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly TriolingoDatabaseContext _context;
        private readonly IConfiguration _configuration;
        public UsersController(TriolingoDatabaseContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;

        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
          if (_context.Users == null)
          {
              return NotFound();
          }
            return await _context.Users.ToListAsync();
        }



        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
          if (_context.Users == null)
          {
              return NotFound();
          }
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // GET: api/Users/5
        [HttpGet("student-info/{id}")]
        public async Task<ActionResult<StudentInfoDto>> GetStudentInfo(int id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            int coursesNum = await _context.StudentCourses.Where(sc => sc.StudentId == id).CountAsync();
            var studentCourseIds = await _context.StudentCourses
                                    .Where(sc => sc.StudentId == id)
                                    .Select(sc => sc.Id)
                                    .ToListAsync();

            // Then, calculate the total points (sum of marks) for those StudentCourse IDs
            int totalPoints = (int)await _context.StudentLessons
                                          .Where(sl => studentCourseIds.Contains(sl.StudentCourseId))
                                          .SumAsync(sl => sl.Mark);
            StudentInfoDto studentInfo = new()
            {
                Id = id,
                AvatarUrl = user.AvatarUrl,
                Email = user.Email,
                FullName = user.FullName,
                Note = user.Note,
                Status = user.Status,
                CoursesNumber = coursesNum,
                TotalPoint = totalPoints
            };


            return studentInfo;
        }

        public class LoginRequest
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }
        // Get user by email and password
        [HttpPost("getUserByEmailPassword")]
        public async Task<ActionResult<User>> GetUserByEmailAndPassword([FromBody] LoginRequest loginRequest)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginRequest.Email && u.Password == loginRequest.Password);
            if (user == null)
            {
                return NotFound();
            }
            var token = GenerateJwtToken(user);
            return Ok(new { Token = token });
            return user;

        }

        // Get user by email
        [HttpGet("getUserByEmail")]
        public async Task<ActionResult<User>> GetUserByEmail(string email)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
          if (_context.Users == null)
          {
              return Problem("Entity set 'TriolingoDatabaseContext.Users'  is null.");
          }
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }
   
        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        [HttpPut("resetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == resetPasswordDto.Email);
            if (user == null)
            {
                return NotFound(new { message = "Email không tồn tại" });
            }

            // Cập nhật mật khẩu mới
            user.Password = resetPasswordDto.NewPassword; // Bạn có thể thêm hash mật khẩu tại đây nếu cần
            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Lỗi khi cập nhật mật khẩu");
            }

            return Ok(new { message = "Cập nhật mật khẩu thành công" });
        }
        private string GenerateJwtToken(User user)
        {
            // Lấy thông tin UserRole của người dùng
            var userRole = _context.UserRoles.Include(ur => ur.User).Include(ur => ur.Setting).FirstOrDefault(ur=>ur.UserId==user.Id); 

            if (userRole == null)
            {
                throw new Exception("User does not have any role assigned.");
            }

            // Tạo danh sách các claims cho JWT
            var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Email), // Email của người dùng
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Mã token
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // UserId
        new Claim("UserId", user.Id.ToString()) // Thêm claim UserId
    };

            // Xác định RoleType và thêm vào claims
            var role = userRole.RoleType == 1 ? "Admin" : (userRole.RoleType == 2 ? "Staff" : "User");

            // Thêm claim Role vào JWT token
            claims.Add(new Claim(ClaimTypes.Role, role));

            // Tạo khóa bảo mật từ cấu hình
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Tạo JWT token
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1), // Token hết hạn sau 1 ngày
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
