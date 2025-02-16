using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRM_API.Models;
using PRM_API.DTO;

namespace PRM_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentCoursesController : ControllerBase
    {
        private readonly TriolingoDatabaseContext _context;

        public StudentCoursesController(TriolingoDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/StudentCourses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentCourse>>> GetStudentCourses()
        {
          if (_context.StudentCourses == null)
          {
              return NotFound();
          }
            return await _context.StudentCourses.ToListAsync();
        }

        // GET: api/StudentCourses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentCourse>> GetStudentCourse(int id)
        {
          if (_context.StudentCourses == null)
          {
              return NotFound();
          }
            var studentCourse = await _context.StudentCourses.FindAsync(id);

            if (studentCourse == null)
            {
                return NotFound();
            }

            return studentCourse;
        }

        // PUT: api/StudentCourses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudentCourse(int id, StudentCourse studentCourse)
        {
            if (id != studentCourse.Id)
            {
                return BadRequest();
            }

            _context.Entry(studentCourse).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentCourseExists(id))
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

        // POST: api/StudentCourses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<StudentCourse>> PostStudentCourse(StudentCourseDto studentCourse)
        {
          if (_context.StudentCourses == null)
          {
              return Problem("Entity set 'TriolingoDatabaseContext.StudentCourses'  is null.");
          }
            StudentCourse newSC = new StudentCourse()
            {
                StudentId = studentCourse.StudentId,
                CourseId = studentCourse.CourseId,
            };

            _context.StudentCourses.Add(newSC);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudentCourse", new { id = newSC.Id }, newSC);
        }

        // DELETE: api/StudentCourses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudentCourse(int id)
        {
            if (_context.StudentCourses == null)
            {
                return NotFound();
            }
            var studentCourse = await _context.StudentCourses.FindAsync(id);
            if (studentCourse == null)
            {
                return NotFound();
            }

            _context.StudentCourses.Remove(studentCourse);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("getListStudentCourse")]
        public async Task<ActionResult<IEnumerable<StudentCourse>>> GetListStudentCourses(int userId, int courseId)
        {
            if (_context.StudentCourses == null)
            {
                return NotFound();
            }
            return await _context.StudentCourses
                .Where(s => s.StudentId == userId && s.CourseId == courseId)
                .ToListAsync();
        }

        [HttpGet("getListStudentCourseByStudentId")]
        public async Task<ActionResult<IEnumerable<StudentCourse>>> GetListStudentCourseByStudentId(int studentId)
        {
            if (_context.StudentCourses == null)
            {
                return NotFound();
            }
            return await _context.StudentCourses
                .Where(s => s.StudentId == studentId)
                .ToListAsync();
        }


        private bool StudentCourseExists(int id)
        {
            return (_context.StudentCourses?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        
    }
}
