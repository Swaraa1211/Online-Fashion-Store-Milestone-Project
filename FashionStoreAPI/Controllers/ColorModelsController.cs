using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FashionStoreAPI.Data;
using FashionStoreAPI.Models;

namespace FashionStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColorModelsController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public ColorModelsController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: api/ColorModels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ColorModel>>> GetColor()
        {
          if (_context.Colors == null)
          {
              return NotFound();
          }
            return await _context.Colors.ToListAsync();
        }

        // GET: api/ColorModels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ColorModel>> GetColorModel(int id)
        {
          if (_context.Colors == null)
          {
              return NotFound();
          }
            var colorModel = await _context.Colors.FindAsync(id);

            if (colorModel == null)
            {
                return NotFound();
            }

            return colorModel;
        }

        // PUT: api/ColorModels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutColorModel(int id, ColorModel colorModel)
        {
            if (id != colorModel.Color_Id)
            {
                return BadRequest();
            }

            _context.Entry(colorModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ColorModelExists(id))
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

        // POST: api/ColorModels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ColorModel>> PostColorModel(ColorModel colorModel)
        {
          if (_context.Colors == null)
          {
              return Problem("Entity set 'ApplicationDBContext.Color'  is null.");
          }
            _context.Colors.Add(colorModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetColorModel", new { id = colorModel.Color_Id }, colorModel);
        }

        // DELETE: api/ColorModels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteColorModel(int id)
        {
            if (_context.Colors == null)
            {
                return NotFound();
            }
            var colorModel = await _context.Colors.FindAsync(id);
            if (colorModel == null)
            {
                return NotFound();
            }

            _context.Colors.Remove(colorModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ColorModelExists(int id)
        {
            return (_context.Colors?.Any(e => e.Color_Id == id)).GetValueOrDefault();
        }
    }
}
