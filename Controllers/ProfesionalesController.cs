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
    public class ProfesionalesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProfesionalesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Profesionales
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Profesionales.Include(p => p.Localidad).Include(p => p.Provincia);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Profesionales/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Profesionales == null)
            {
                return NotFound();
            }

            var profesional = await _context.Profesionales
                .Include(p => p.Localidad)
                .Include(p => p.Provincia)
                .FirstOrDefaultAsync(m => m.ProfesionalId == id);
            if (profesional == null)
            {
                return NotFound();
            }

            return View(profesional);
        }

        // GET: Profesionales/Create
        public IActionResult Create()
        {
            ViewData["LocalidadId"] = new SelectList(_context.Localidades, "LocalidadId", "LocalidadId");
            ViewData["ProvinciaId"] = new SelectList(_context.Provincias, "ProvinciaId", "ProvinciaId");
            return View();
        }

        // POST: Profesionales/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProfesionalId,Apellido,Nombre,FechaNacimiento,TipoDocumento,NumeroDocumento,Calle,Altura,ProvinciaId,LocalidadId,CodigoPostal,CorreoElectronico,Telefono,Especialidad")] Profesional profesional)
        {
            if (ModelState.IsValid)
            {
                _context.Add(profesional);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LocalidadId"] = new SelectList(_context.Localidades, "LocalidadId", "LocalidadId", profesional.LocalidadId);
            ViewData["ProvinciaId"] = new SelectList(_context.Provincias, "ProvinciaId", "ProvinciaId", profesional.ProvinciaId);
            return View(profesional);
        }

        // GET: Profesionales/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Profesionales == null)
            {
                return NotFound();
            }

            var profesional = await _context.Profesionales.FindAsync(id);
            if (profesional == null)
            {
                return NotFound();
            }
            ViewData["LocalidadId"] = new SelectList(_context.Localidades, "LocalidadId", "LocalidadId", profesional.LocalidadId);
            ViewData["ProvinciaId"] = new SelectList(_context.Provincias, "ProvinciaId", "ProvinciaId", profesional.ProvinciaId);
            return View(profesional);
        }

        // POST: Profesionales/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProfesionalId,Apellido,Nombre,FechaNacimiento,TipoDocumento,NumeroDocumento,Calle,Altura,ProvinciaId,LocalidadId,CodigoPostal,CorreoElectronico,Telefono,Especialidad")] Profesional profesional)
        {
            if (id != profesional.ProfesionalId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(profesional);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProfesionalExists(profesional.ProfesionalId))
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
            ViewData["LocalidadId"] = new SelectList(_context.Localidades, "LocalidadId", "LocalidadId", profesional.LocalidadId);
            ViewData["ProvinciaId"] = new SelectList(_context.Provincias, "ProvinciaId", "ProvinciaId", profesional.ProvinciaId);
            return View(profesional);
        }

        // GET: Profesionales/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Profesionales == null)
            {
                return NotFound();
            }

            var profesional = await _context.Profesionales
                .Include(p => p.Localidad)
                .Include(p => p.Provincia)
                .FirstOrDefaultAsync(m => m.ProfesionalId == id);
            if (profesional == null)
            {
                return NotFound();
            }

            return View(profesional);
        }

        // POST: Profesionales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Profesionales == null)
            {
                return Problem("Entity set 'ApplicationDbContext.profesionales'  is null.");
            }
            var profesional = await _context.Profesionales.FindAsync(id);
            if (profesional != null)
            {
                _context.Profesionales.Remove(profesional);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProfesionalExists(int id)
        {
          return (_context.Profesionales?.Any(e => e.ProfesionalId == id)).GetValueOrDefault();
        }
    }
}
