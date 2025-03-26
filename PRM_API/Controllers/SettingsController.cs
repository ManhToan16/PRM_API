using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRM_API.DTO;
using PRM_API.Models;

namespace PRM_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly TriolingoDatabaseContext _context;

        public SettingsController(TriolingoDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Settings
        [HttpGet]
        public async Task<ActionResult<SettingsDTO>> GetSettings()
        {
            if (_context.Settings == null)
            {
                return NotFound();
            }

            // Lấy dữ liệu và ánh xạ sang DTO
            var settings = await _context.Settings
                .Select(s => new SettingsDTO
                {
                    Id = s.Id,
                    Name = s.Name,
                    Status = s.Status,
             
                })
                .ToListAsync();

            return Ok(settings); // Trả về danh sách SettingsDTO
        }
        [HttpGet("GetRoles")]
        public async Task<ActionResult<SettingsDTO>> GetRoles()
        {
            if (_context.Settings == null)
            {
                return NotFound();
            }

            // Lấy dữ liệu và ánh xạ sang DTO
            var settings = await _context.Settings.Where(s => s.Id == 2 || s.Id == 3 || s.Id == 4)
                .Select(s => new SettingsDTO
                {
                    Id = s.Id,
                    Name = s.Name,
                    Status = s.Status,

                })
                .ToListAsync();

            return Ok(settings); // Trả về danh sách SettingsDTO
        }

        // GET: api/Settings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Setting>> GetSetting(int id)
        {
          if (_context.Settings == null)
          {
              return NotFound();
          }
            var setting = await _context.Settings.FindAsync(id);

            if (setting == null)
            {
                return NotFound();
            }

            return setting;
        }

        // PUT: api/Settings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSetting(int id, Setting setting)
        {
            if (id != setting.Id)
            {
                return BadRequest();
            }

            _context.Entry(setting).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SettingExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(id);
        }

        // POST: api/Settings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Setting>> PostSetting(Setting setting)
        {
          if (_context.Settings == null)
          {
              return Problem("Entity set 'TriolingoDatabaseContext.Settings'  is null.");
          }
            _context.Settings.Add(setting);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSetting", new { id = setting.Id }, setting);
        }

        // DELETE: api/Settings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSetting(int id)
        {
            if (_context.Settings == null)
            {
                return NotFound();
            }
            var setting = await _context.Settings.FindAsync(id);
            if (setting == null)
            {
                return NotFound();
            }

            _context.Settings.Remove(setting);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SettingExists(int id)
        {
            return (_context.Settings?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
