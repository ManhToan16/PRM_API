using Microsoft.AspNetCore.Mvc;
using PRM_API.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace PRM_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {
        private readonly TriolingoDatabaseContext _context;

        public UserRoleController(TriolingoDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/UserRole
        [HttpGet]
        public ActionResult<IEnumerable<UserRole>> GetUserRoles()
        {
            var userRoles = _context.UserRoles.Include(ur => ur.User).Include(ur => ur.Setting).ToList();
            return Ok(userRoles);
        }

        // GET: api/UserRole/{id}
        [HttpGet("{id}")]
        public ActionResult<UserRole> GetUserRole(int id)
        {
            var userRole = _context.UserRoles.Include(ur => ur.User).Include(ur => ur.Setting)
                                              .FirstOrDefault(ur => ur.Id == id);
            if (userRole == null)
            {
                return NotFound();
            }

            return Ok(userRole);
        }

        // POST: api/UserRole
        [HttpPost]
        public ActionResult<UserRole> PostUserRole(UserRole userRole)
        {
            _context.UserRoles.Add(userRole);
            _context.SaveChanges();

            return CreatedAtAction("GetUserRole", new { id = userRole.Id }, userRole);
        }

        // PUT: api/UserRole/{id}
        [HttpPut("{id}")]
        public IActionResult PutUserRole(int id, UserRole userRole)
        {
            if (id != userRole.Id)
            {
                return BadRequest();
            }

            _context.Entry(userRole).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserRoleExists(id))
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

        // DELETE: api/UserRole/{id}
        [HttpDelete("{id}")]
        public ActionResult<UserRole> DeleteUserRole(int id)
        {
            var userRole = _context.UserRoles.Find(id);
            if (userRole == null)
            {
                return NotFound();
            }

            _context.UserRoles.Remove(userRole);
            _context.SaveChanges();

            return Ok(userRole);
        }

        private bool UserRoleExists(int id)
        {
            return _context.UserRoles.Any(ur => ur.Id == id);
        }

        // GET: api/UserRole/GetRoleByUserId/{userId}
        [HttpGet("GetRoleByUserId/{userId}")]
        public ActionResult<IEnumerable<UserRole>> GetRoleByUserId(int userId)
        {
            var userRoles = _context.UserRoles.Where(ur => ur.UserId == userId).Include(ur => ur.Setting).ToList();
            if (userRoles == null || !userRoles.Any())
            {
                return NotFound();
            }

            return Ok(userRoles);
        }
    }
}
