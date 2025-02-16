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
    public class StudentLessonsController : ControllerBase
    {
        private readonly TriolingoDatabaseContext _context;

        public StudentLessonsController(TriolingoDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/StudentLessons
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentLesson>>> GetStudentLessons()
        {
            if (_context.StudentLessons == null)
            {
                return NotFound();
            }
            return await _context.StudentLessons.ToListAsync();
        }

        // GET: api/StudentLessons/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentLesson>> GetStudentLesson(int id)
        {
            if (_context.StudentLessons == null)
            {
                return NotFound();
            }
            var studentLesson = await _context.StudentLessons.FindAsync(id);

            if (studentLesson == null)
            {
                return NotFound();
            }

            return studentLesson;
        }

        // PUT: api/StudentLessons/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudentLesson(int id, StudentLesson studentLesson)
        {
            if (id != studentLesson.Id)
            {
                return BadRequest();
            }

            _context.Entry(studentLesson).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentLessonExists(id))
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

        // POST: api/StudentLessons
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<StudentLesson>> PostStudentLesson(StudentLessonDto studentLessonDto)
        {
            if (_context.StudentLessons == null)
            {
                return Problem("Entity set 'TriolingoDatabaseContext.StudentLessons'  is null.");
            }

            StudentLesson studentLesson = new()
            {
                LessionId = studentLessonDto.LessionId,
                Mark = studentLessonDto.Mark,
                StudentCourseId = studentLessonDto.StudentCourseId,
            };

            _context.StudentLessons.Add(studentLesson);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudentLesson", new { id = studentLesson.Id }, studentLesson);
        }

        // DELETE: api/StudentLessons/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudentLesson(int id)
        {
            if (_context.StudentLessons == null)
            {
                return NotFound();
            }
            var studentLesson = await _context.StudentLessons.FindAsync(id);
            if (studentLesson == null)
            {
                return NotFound();
            }

            _context.StudentLessons.Remove(studentLesson);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StudentLessonExists(int id)
        {
            return (_context.StudentLessons?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [HttpGet("getListStudentLesson")]
        public async Task<ActionResult<IEnumerable<StudentLesson>>> getListStudentLesson(int unitId, int studentCourseId)
        {
            if (_context.StudentLessons == null)
            {
                return NotFound();
            }
            return await _context.StudentLessons
                .Where(sl => _context.Lessons
                                .Where(l => l.UnitId == unitId)
                                .Select(l => l.Id)
                                .Contains(sl.LessionId) &&
                         sl.StudentCourseId == studentCourseId)
            .ToListAsync();
        }

        [HttpGet("getStudentLessonDetail")]
        public async Task<ActionResult<StudentLesson>> getStudentLessonDetail(int studentCourseId, int lessonId)
        {
            if (_context.StudentLessons == null)
            {
                return NotFound();
            }
            var a = await _context.StudentLessons
                .FirstOrDefaultAsync(s => s.StudentCourseId == studentCourseId && s.LessionId == lessonId);
            if (a == null) { return NotFound(); }
            return a;
                
        }

        [HttpPut("updateStudentLesson")]
        public async Task<IActionResult> updateStudentLesson([FromBody] StudentLesson studentLesson)
        {

            var existingStudentLesson = await _context.StudentLessons
                .FirstOrDefaultAsync(sl => sl.LessionId == studentLesson.LessionId && sl.StudentCourseId == studentLesson.StudentCourseId);

            if (existingStudentLesson == null)
            {
                return NotFound("StudentLesson not found.");
            }

            existingStudentLesson.Mark = studentLesson.Mark;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Failed to update the student lesson.");
            }

            return NoContent();
        }


    }
}
