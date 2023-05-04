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
    public class SizeModelsController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public SizeModelsController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: api/SizeModels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SizeModel>>> GetSize()
        {
          if (_context.Size == null)
          {
              return NotFound();
          }
            return await _context.Size.ToListAsync();
        }

        // GET: api/SizeModels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SizeModel>> GetSizeModel(int id)
        {
          if (_context.Size == null)
          {
              return NotFound();
          }
            var sizeModel = await _context.Size.FindAsync(id);

            if (sizeModel == null)
            {
                return NotFound();
            }

            return sizeModel;
        }

        // PUT: api/SizeModels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSizeModel(int id, SizeModel sizeModel)
        {
            if (id != sizeModel.Size_Id)
            {
                return BadRequest();
            }

            _context.Entry(sizeModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SizeModelExists(id))
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

        // POST: api/SizeModels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SizeModel>> PostSizeModel(SizeModel sizeModel)
        {
          if (_context.Size == null)
          {
              return Problem("Entity set 'ApplicationDBContext.Size'  is null.");
          }
            _context.Size.Add(sizeModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSizeModel", new { id = sizeModel.Size_Id }, sizeModel);
        }

        // DELETE: api/SizeModels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSizeModel(int id)
        {
            if (_context.Size == null)
            {
                return NotFound();
            }
            var sizeModel = await _context.Size.FindAsync(id);
            if (sizeModel == null)
            {
                return NotFound();
            }

            _context.Size.Remove(sizeModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SizeModelExists(int id)
        {
            return (_context.Size?.Any(e => e.Size_Id == id)).GetValueOrDefault();
        }
    }
}
