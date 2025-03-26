using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRM_API.Models;

namespace PRM_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitsController : ControllerBase
    {
        private readonly TriolingoDatabaseContext _context;

        public UnitsController(TriolingoDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Units
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Unit>>> GetUnits()
        {
            if (_context.Units == null)
            {
                return NotFound();
            }
            return await _context.Units.ToListAsync();
        }

        // GET: api/Units/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Unit>> GetUnit(int id)
        {
            if (_context.Units == null)
            {
                return NotFound();
            }
            var unit = await _context.Units.FindAsync(id);

            if (unit == null)
            {
                return NotFound();
            }

            return unit;
        }

        // PUT: api/Units/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUnit(int id, Unit unit)
        {
            if (id != unit.Id)
            {
                return BadRequest();
            }

            _context.Entry(unit).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UnitExists(id))
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

        // POST: api/Units
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Unit>> PostUnit(Unit unit)
        {
            if (_context.Units == null)
            {
                return Problem("Entity set 'TriolingoDatabaseContext.Units' is null.");
            }

            try
            {
                if (string.IsNullOrEmpty(unit.Name) || string.IsNullOrEmpty(unit.Description))
                {
                    return BadRequest("Name và Description là bắt buộc");
                }

                if (!_context.Courses.Any(c => c.Id == unit.CourseId))
                {
                    return BadRequest($"CourseId {unit.CourseId} không tồn tại");
                }

                if (_context.Units.Any(u => u.Name == unit.Name && u.CourseId == unit.CourseId))
                {
                    return BadRequest($"Unit với tên '{unit.Name}' đã tồn tại trong khóa học này");
                }

                Console.WriteLine($"Received Unit: Id={unit.Id}, Name={unit.Name}, CourseId={unit.CourseId}, Course={unit.Course?.Id}");

                unit.Course = null;

                _context.Units.Add(unit);
                await _context.SaveChangesAsync();

                // Làm mới context để đảm bảo dữ liệu được đồng bộ
                await _context.Entry(unit).ReloadAsync();

                return CreatedAtAction("GetUnit", new { id = unit.Id }, unit);
            }
            catch (DbUpdateException ex)
            {
                var innerException = ex.InnerException?.Message ?? ex.Message;
                Console.WriteLine($"DbUpdateException: {innerException}");
                return BadRequest($"Lỗi khi thêm unit: {innerException}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return BadRequest($"Lỗi khi thêm unit: {ex.Message}");
            }
        }

        // DELETE: api/Units/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUnit(int id)
        {
            if (_context.Units == null)
            {
                return NotFound();
            }
            var unit = await _context.Units.FindAsync(id);
            if (unit == null)
            {
                return NotFound();
            }

            _context.Units.Remove(unit);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UnitExists(int id)
        {
            return (_context.Units?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        [HttpGet("getListUnitByCourseId")]
        public async Task<ActionResult<IEnumerable<Unit>>> GetListUnitByCourseId(int courseId)
        {
            if (_context.Units == null)
            {
                return NotFound();
            }

            // Kiểm tra vai trò người dùng (giả định bạn có cách lấy vai trò từ token)
            var userRole = User.Claims.FirstOrDefault(c => c.Type == "role")?.Value; // Ví dụ: "Admin" hoặc "Student"

            IQueryable<Unit> unitsQuery = _context.Units
                .Where(u => courseId != 0 ? u.CourseId == courseId : true)
                .Include(u => u.Lessons);

            // Nếu là student, chỉ lấy unit có Status > 0
            if (userRole == "Student")
            {
                unitsQuery = unitsQuery.Where(u => u.Status > 0);
            }
            // Nếu là admin, lấy tất cả unit (bất kể Status)

            var units = await unitsQuery.ToListAsync();

            // Log dữ liệu trả về
            Console.WriteLine($"Units returned for CourseId {courseId}: {units.Count} units");
            foreach (var unit in units)
            {
                Console.WriteLine($"Unit: Id={unit.Id}, Name={unit.Name}, Status={unit.Status}");
            }

            return units;
        }

        [HttpGet("getLessonsCountByUnitId/{unitId}")]
        public async Task<ActionResult<int>> GetLessonsCountByUnitId(int unitId)
        {
            if (_context.Units == null || _context.Lessons == null)
            {
                return NotFound();
            }
            var count = await _context.Lessons
                .Where(l => l.UnitId == unitId)
                .CountAsync();
            return count;
        }
    }
}
