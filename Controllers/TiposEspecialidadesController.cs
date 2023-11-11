using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AgendaDelConsultorio.Data;
using AgendaDelConsultorio.Models;

namespace AgendaDelConsultorio.Controllers
{
    public class TiposEspecialidadesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TiposEspecialidadesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TiposEspecialidades
        public async Task<IActionResult> Index()
        {
              return _context.tiposespecialidads != null ? 
                          View(await _context.tiposespecialidads.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.tiposespecialidads'  is null.");
        }

        // GET: TiposEspecialidades/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.tiposespecialidads == null)
            {
                return NotFound();
            }

            var tiposespecialidad = await _context.tiposespecialidads
                .FirstOrDefaultAsync(m => m.TipoEspecialidadId == id);
            if (tiposespecialidad == null)
            {
                return NotFound();
            }

            return View(tiposespecialidad);
        }

        // GET: TiposEspecialidades/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TiposEspecialidades/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TipoEspecialidadId,Descripcion")] tiposespecialidad tiposespecialidad)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tiposespecialidad);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tiposespecialidad);
        }

        // GET: TiposEspecialidades/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.tiposespecialidads == null)
            {
                return NotFound();
            }

            var tiposespecialidad = await _context.tiposespecialidads.FindAsync(id);
            if (tiposespecialidad == null)
            {
                return NotFound();
            }
            return View(tiposespecialidad);
        }

        // POST: TiposEspecialidades/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TipoEspecialidadId,Descripcion")] tiposespecialidad tiposespecialidad)
        {
            if (id != tiposespecialidad.TipoEspecialidadId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tiposespecialidad);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!tiposespecialidadExists(tiposespecialidad.TipoEspecialidadId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(tiposespecialidad);
        }

        // GET: TiposEspecialidades/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.tiposespecialidads == null)
            {
                return NotFound();
            }

            var tiposespecialidad = await _context.tiposespecialidads
                .FirstOrDefaultAsync(m => m.TipoEspecialidadId == id);
            if (tiposespecialidad == null)
            {
                return NotFound();
            }

            return View(tiposespecialidad);
        }

        // POST: TiposEspecialidades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.tiposespecialidads == null)
            {
                return Problem("Entity set 'ApplicationDbContext.tiposespecialidads'  is null.");
            }
            var tiposespecialidad = await _context.tiposespecialidads.FindAsync(id);
            if (tiposespecialidad != null)
            {
                _context.tiposespecialidads.Remove(tiposespecialidad);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool tiposespecialidadExists(int id)
        {
          return (_context.tiposespecialidads?.Any(e => e.TipoEspecialidadId == id)).GetValueOrDefault();
        }
    }
}
