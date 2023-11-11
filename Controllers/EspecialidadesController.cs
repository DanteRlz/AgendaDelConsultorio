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
    public class EspecialidadesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EspecialidadesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Especialidades
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.especialidades.Include(e => e.TipoEspecialidad);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Especialidades/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.especialidades == null)
            {
                return NotFound();
            }

            var especialidad = await _context.especialidades
                .Include(e => e.TipoEspecialidad)
                .FirstOrDefaultAsync(m => m.EspecialidadId == id);
            if (especialidad == null)
            {
                return NotFound();
            }

            return View(especialidad);
        }

        // GET: Especialidades/Create
        public IActionResult Create()
        {
            ViewData["TipoEspecialidadId"] = new SelectList(_context.tiposespecialidads, "TipoEspecialidadId", "TipoEspecialidadId");
            return View();
        }

        // POST: Especialidades/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EspecialidadId,Descripcion,Precio,Duracion,Observacion,TipoEspecialidadId")] Especialidad especialidad)
        {
            if (ModelState.IsValid)
            {
                _context.Add(especialidad);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TipoEspecialidadId"] = new SelectList(_context.tiposespecialidads, "TipoEspecialidadId", "TipoEspecialidadId", especialidad.TipoEspecialidadId);
            return View(especialidad);
        }

        // GET: Especialidades/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.especialidades == null)
            {
                return NotFound();
            }

            var especialidad = await _context.especialidades.FindAsync(id);
            if (especialidad == null)
            {
                return NotFound();
            }
            ViewData["TipoEspecialidadId"] = new SelectList(_context.tiposespecialidads, "TipoEspecialidadId", "TipoEspecialidadId", especialidad.TipoEspecialidadId);
            return View(especialidad);
        }

        // POST: Especialidades/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EspecialidadId,Descripcion,Precio,Duracion,Observacion,TipoEspecialidadId")] Especialidad especialidad)
        {
            if (id != especialidad.EspecialidadId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(especialidad);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EspecialidadExists(especialidad.EspecialidadId))
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
            ViewData["TipoEspecialidadId"] = new SelectList(_context.tiposespecialidads, "TipoEspecialidadId", "TipoEspecialidadId", especialidad.TipoEspecialidadId);
            return View(especialidad);
        }

        // GET: Especialidades/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.especialidades == null)
            {
                return NotFound();
            }

            var especialidad = await _context.especialidades
                .Include(e => e.TipoEspecialidad)
                .FirstOrDefaultAsync(m => m.EspecialidadId == id);
            if (especialidad == null)
            {
                return NotFound();
            }

            return View(especialidad);
        }

        // POST: Especialidades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.especialidades == null)
            {
                return Problem("Entity set 'ApplicationDbContext.especialidades'  is null.");
            }
            var especialidad = await _context.especialidades.FindAsync(id);
            if (especialidad != null)
            {
                _context.especialidades.Remove(especialidad);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EspecialidadExists(int id)
        {
          return (_context.especialidades?.Any(e => e.EspecialidadId == id)).GetValueOrDefault();
        }
    }
}
