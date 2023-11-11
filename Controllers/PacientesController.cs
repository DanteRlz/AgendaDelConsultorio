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
    public class PacientesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PacientesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Pacientes
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.pacientes.Include(p => p.Localidad).Include(p => p.Provincia);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Pacientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.pacientes == null)
            {
                return NotFound();
            }

            var paciente = await _context.pacientes
                .Include(p => p.Localidad)
                .Include(p => p.Provincia)
                .FirstOrDefaultAsync(m => m.PacienteId == id);
            if (paciente == null)
            {
                return NotFound();
            }

            return View(paciente);
        }

        // GET: Pacientes/Create
        public IActionResult Create()
        {
            ViewData["LocalidadId"] = new SelectList(_context.localidades, "Localidadid", "Descripcion");
            ViewData["ProvinciaId"] = new SelectList(_context.provincias, "ProvinciaId", "Descripcion");
            return View();
        }

        // POST: Pacientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PacienteId,Apellido,Nombre,FechaNacimiento,TipoDocumento,NumeroDocumento,Calle,Altura,ProvinciaId,LocalidadId,CodigoPostal,CorreoElectronico,Telefono")] paciente paciente)
        {
            if (ModelState.IsValid)
            {
                _context.Add(paciente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LocalidadId"] = new SelectList(_context.localidades, "Localidadid", "Descripcion", paciente.LocalidadId);
            ViewData["ProvinciaId"] = new SelectList(_context.provincias, "ProvinciaId", "Descripcion", paciente.ProvinciaId);
            return View(paciente);
        }

        // GET: Pacientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.pacientes == null)
            {
                return NotFound();
            }

            var paciente = await _context.pacientes.FindAsync(id);
            if (paciente == null)
            {
                return NotFound();
            }
            ViewData["LocalidadId"] = new SelectList(_context.localidades, "Localidadid", "Descripcion", paciente.LocalidadId);
            ViewData["ProvinciaId"] = new SelectList(_context.provincias, "ProvinciaId", "Descripcion", paciente.ProvinciaId);
            return View(paciente);
        }

        // POST: Pacientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PacienteId,Apellido,Nombre,FechaNacimiento,TipoDocumento,NumeroDocumento,Calle,Altura,ProvinciaId,LocalidadId,CodigoPostal,CorreoElectronico,Telefono")] paciente paciente)
        {
            if (id != paciente.PacienteId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(paciente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!pacienteExists(paciente.PacienteId))
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
            ViewData["LocalidadId"] = new SelectList(_context.localidades, "Localidadid", "Descripcion", paciente.LocalidadId);
            ViewData["ProvinciaId"] = new SelectList(_context.provincias, "ProvinciaId", "Descripcion", paciente.ProvinciaId);
            return View(paciente);
        }

        // GET: Pacientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.pacientes == null)
            {
                return NotFound();
            }

            var paciente = await _context.pacientes
                .Include(p => p.Localidad)
                .Include(p => p.Provincia)
                .FirstOrDefaultAsync(m => m.PacienteId == id);
            if (paciente == null)
            {
                return NotFound();
            }

            return View(paciente);
        }

        // POST: Pacientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.pacientes == null)
            {
                return Problem("Entity set 'ApplicationDbContext.pacientes'  is null.");
            }
            var paciente = await _context.pacientes.FindAsync(id);
            if (paciente != null)
            {
                _context.pacientes.Remove(paciente);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool pacienteExists(int id)
        {
          return (_context.pacientes?.Any(e => e.PacienteId == id)).GetValueOrDefault();
        }
    }
}
