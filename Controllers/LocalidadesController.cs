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
    public class LocalidadesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LocalidadesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Localidades
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.localidades.Include(l => l.Provincia);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Localidades/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.localidades == null)
            {
                return NotFound();
            }

            var localidad = await _context.localidades
                .Include(l => l.Provincia)
                .FirstOrDefaultAsync(m => m.Localidadid == id);
            if (localidad == null)
            {
                return NotFound();
            }

            return View(localidad);
        }

        // GET: Localidades/Create
        public IActionResult Create()
        {
            ViewData["ProvinciaId"] = new SelectList(_context.provincias, "ProvinciaId", "ProvinciaId");
            return View();
        }

        // POST: Localidades/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Localidadid,ProvinciaId,Descripcion")] Localidad localidad)
        {
            if (ModelState.IsValid)
            {
                _context.Add(localidad);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProvinciaId"] = new SelectList(_context.provincias, "ProvinciaId", "ProvinciaId", localidad.ProvinciaId);
            return View(localidad);
        }

        // GET: Localidades/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.localidades == null)
            {
                return NotFound();
            }

            var localidad = await _context.localidades.FindAsync(id);
            if (localidad == null)
            {
                return NotFound();
            }
            ViewData["ProvinciaId"] = new SelectList(_context.provincias, "ProvinciaId", "ProvinciaId", localidad.ProvinciaId);
            return View(localidad);
        }

        // POST: Localidades/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Localidadid,ProvinciaId,Descripcion")] Localidad localidad)
        {
            if (id != localidad.Localidadid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(localidad);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LocalidadExists(localidad.Localidadid))
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
            ViewData["ProvinciaId"] = new SelectList(_context.provincias, "ProvinciaId", "ProvinciaId", localidad.ProvinciaId);
            return View(localidad);
        }

        // GET: Localidades/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.localidades == null)
            {
                return NotFound();
            }

            var localidad = await _context.localidades
                .Include(l => l.Provincia)
                .FirstOrDefaultAsync(m => m.Localidadid == id);
            if (localidad == null)
            {
                return NotFound();
            }

            return View(localidad);
        }

        // POST: Localidades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.localidades == null)
            {
                return Problem("Entity set 'ApplicationDbContext.localidades'  is null.");
            }
            var localidad = await _context.localidades.FindAsync(id);
            if (localidad != null)
            {
                _context.localidades.Remove(localidad);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LocalidadExists(int id)
        {
          return (_context.localidades?.Any(e => e.Localidadid == id)).GetValueOrDefault();
        }
    }
}
