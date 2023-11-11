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
    public class TurnosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TurnosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Turnos
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.turnos.Include(t => t.Especialidad).Include(t => t.EstadoTurno).Include(t => t.Paciente);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Turnos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.turnos == null)
            {
                return NotFound();
            }

            var turno = await _context.turnos
                .Include(t => t.Especialidad)
                .Include(t => t.EstadoTurno)
                .Include(t => t.Paciente)
                .FirstOrDefaultAsync(m => m.TurnoId == id);
            if (turno == null)
            {
                return NotFound();
            }

            return View(turno);
        }

        // GET: Turnos/Create
        public IActionResult Create()
        {
            ViewData["EspecialidadId"] = new SelectList(_context.especialidades, "EspecialidadId", "EspecialidadId");
            ViewData["EstadoTurnoId"] = new SelectList(_context.estadosturnos, "EstadoTurnoId", "EstadoTurnoId");
            ViewData["PacienteId"] = new SelectList(_context.pacientes, "PacienteId", "PacienteId");
            return View();
        }

        // POST: Turnos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TurnoId,FechaTurno,HoraTurno,EstadoTurnoId,PacienteId,EspecialidadId,Observacion")] turno turno)
        {
            if (ModelState.IsValid)
            {
                _context.Add(turno);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EspecialidadId"] = new SelectList(_context.especialidades, "EspecialidadId", "EspecialidadId", turno.EspecialidadId);
            ViewData["EstadoTurnoId"] = new SelectList(_context.estadosturnos, "EstadoTurnoId", "EstadoTurnoId", turno.EstadoTurnoId);
            ViewData["PacienteId"] = new SelectList(_context.pacientes, "PacienteId", "PacienteId", turno.PacienteId);
            return View(turno);
        }

        // GET: Turnos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.turnos == null)
            {
                return NotFound();
            }

            var turno = await _context.turnos.FindAsync(id);
            if (turno == null)
            {
                return NotFound();
            }
            ViewData["EspecialidadId"] = new SelectList(_context.especialidades, "EspecialidadId", "EspecialidadId", turno.EspecialidadId);
            ViewData["EstadoTurnoId"] = new SelectList(_context.estadosturnos, "EstadoTurnoId", "EstadoTurnoId", turno.EstadoTurnoId);
            ViewData["PacienteId"] = new SelectList(_context.pacientes, "PacienteId", "PacienteId", turno.PacienteId);
            return View(turno);
        }

        // POST: Turnos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TurnoId,FechaTurno,HoraTurno,EstadoTurnoId,PacienteId,EspecialidadId,Observacion")] turno turno)
        {
            if (id != turno.TurnoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(turno);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!turnoExists(turno.TurnoId))
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
            ViewData["EspecialidadId"] = new SelectList(_context.especialidades, "EspecialidadId", "EspecialidadId", turno.EspecialidadId);
            ViewData["EstadoTurnoId"] = new SelectList(_context.estadosturnos, "EstadoTurnoId", "EstadoTurnoId", turno.EstadoTurnoId);
            ViewData["PacienteId"] = new SelectList(_context.pacientes, "PacienteId", "PacienteId", turno.PacienteId);
            return View(turno);
        }

        // GET: Turnos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.turnos == null)
            {
                return NotFound();
            }

            var turno = await _context.turnos
                .Include(t => t.Especialidad)
                .Include(t => t.EstadoTurno)
                .Include(t => t.Paciente)
                .FirstOrDefaultAsync(m => m.TurnoId == id);
            if (turno == null)
            {
                return NotFound();
            }

            return View(turno);
        }

        // POST: Turnos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.turnos == null)
            {
                return Problem("Entity set 'ApplicationDbContext.turnos'  is null.");
            }
            var turno = await _context.turnos.FindAsync(id);
            if (turno != null)
            {
                _context.turnos.Remove(turno);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool turnoExists(int id)
        {
          return (_context.turnos?.Any(e => e.TurnoId == id)).GetValueOrDefault();
        }
    }
}
