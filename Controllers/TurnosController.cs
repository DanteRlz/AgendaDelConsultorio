using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AgendaDelConsultorio.Data;
using AgendaDelConsultorio.Models;
using System.Globalization;

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
            var PacienteId = 18;
            var paciente = await _context.Pacientes
                .Include(p => p.Localidad)
                .Include(p => p.Provincia)
                .FirstOrDefaultAsync(m => m.PacienteId == PacienteId);

            ViewData["SelectListTipoEspecialidades"] = new SelectList(_context.TiposEspecialidades, "TipoEspecialidadId", "Descripcion");
            ViewData["SelectListEspecialidades"] = new SelectList(_context.Especialidades, "EspecialidadId", "Descripcion");

            var reservaTurno = new ReservaTurno();
            reservaTurno.PacienteId = PacienteId;
            reservaTurno.Paciente = paciente;

            var fechas = ObtenerFechasLibresDelMesActual();
            var fechasFormateadas = fechas.Select(fecha => fecha.ToString("dd/mm/yyyy").ToList());

            ViewData["SelectListFechas"] = new SelectList(fechasFormateadas);

            var fechaHoy = fechas[0].ToString("dd/mm/yyyy");
            var listaHoras = ObtenerHorasLibresPorFecha(fechaHoy);

            ViewData["SelectListHoras"] = new SelectList(listaHoras);

            return View(reservaTurno);
        }

        [HttpGet]

        public async Task<IActionResult> ObtenerEspecialidadPorTipoEspecialidad(int TipoEspecialidadId)
        {
            var especialidades = await _context.Especialidades
                .Where(e => e.TipoEspecialidadId == TipoEspecialidadId)
                .Select(e => new { e.EspecialidadId, e.Descripcion })
                .ToListAsync();

            return Json(especialidades);
        }

        public List<DateTime> ObtenerFechasLibresDelMesActual()
        {
            var diaDeHoy = DateTime.Now.Date;
            var primerDiaMesVigente = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var ultimoDiaMesVigente = primerDiaMesVigente.AddMonths(1).AddDays(-1);

            const int estadoTurnoIdLibre = 1;
            var fechas = _context.Turnos
                .Where(f => f.FechaTurno.Date == diaDeHoy
                            && f.FechaTurno.Date < ultimoDiaMesVigente
                            && f.EstadoTurnoId == estadoTurnoIdLibre)
                .GroupBy(f => f.FechaTurno.Date)
                .OrderBy(g => g.Key)
                .Select(g => g.Key)
                .ToList();

            return fechas;
        }

        public List<String> ObtenerHorasLibresPorFecha(String fecha)
        {
            var fechaTurno = DateTime.ParseExact(fecha,"dd/mm/yyyy",
                CultureInfo.InvariantCulture);
            const int estadoTurnoIdLibre = 1;

            var horas = from t in _context.Turnos
                        where t.FechaTurno.Date == fechaTurno.Date
                                                && t.EstadoTurnoId == estadoTurnoIdLibre
                        orderby t.HoraTurno
                        select t.HoraTurno;

            return horas.ToList().Select(h => h.ToString(@"hh\:mm")).ToList();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReservarTurno(ReservaTurno model)
        {
            const int estadoTurnoIdLibre = 1;

            var turnoExistente = await (from t in _context.Turnos
                                        where t.FechaTurno == model.FechaTurno
                                              && t.HoraTurno == model.HoraTurno
                                              && t.EstadoTurnoId == estadoTurnoIdLibre
                                        select t).FirstOrDefaultAsync();

            if (turnoExistente == null)
            {
                turnoExistente.PacienteId = model.PacienteId;
                turnoExistente.EspecialidadId = model.EspecialidadId;
                turnoExistente.Observacion = model.Observacion;
                turnoExistente.EstadoTurnoId = 2;

                _context.Update(turnoExistente);
                await _context.SaveChangesAsync(); 

                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Turnos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Turnos == null)
            {
                return NotFound();
            }

            var turno = await _context.Turnos
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
            ViewData["EspecialidadId"] = new SelectList(_context.Especialidades, "EspecialidadId", "EspecialidadId");
            ViewData["EstadoTurnoId"] = new SelectList(_context.EstadosTurnos, "EstadoTurnoId", "EstadoTurnoId");
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "PacienteId", "PacienteId");
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
            ViewData["EspecialidadId"] = new SelectList(_context.Especialidades, "EspecialidadId", "EspecialidadId", turno.EspecialidadId);
            ViewData["EstadoTurnoId"] = new SelectList(_context.EstadosTurnos, "EstadoTurnoId", "EstadoTurnoId", turno.EstadoTurnoId);
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "PacienteId", "PacienteId", turno.PacienteId);
            return View(turno);
        }

        // GET: Turnos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Turnos == null)
            {
                return NotFound();
            }

            var turno = await _context.Turnos.FindAsync(id);
            if (turno == null)
            {
                return NotFound();
            }
            ViewData["EspecialidadId"] = new SelectList(_context.Especialidades, "EspecialidadId", "EspecialidadId", turno.EspecialidadId);
            ViewData["EstadoTurnoId"] = new SelectList(_context.EstadosTurnos, "EstadoTurnoId", "EstadoTurnoId", turno.EstadoTurnoId);
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "PacienteId", "PacienteId", turno.PacienteId);
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
            ViewData["EspecialidadId"] = new SelectList(_context.Especialidades, "EspecialidadId", "EspecialidadId", turno.EspecialidadId);
            ViewData["EstadoTurnoId"] = new SelectList(_context.EstadosTurnos, "EstadoTurnoId", "EstadoTurnoId", turno.EstadoTurnoId);
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "PacienteId", "PacienteId", turno.PacienteId);
            return View(turno);
        }

        // GET: Turnos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Turnos == null)
            {
                return NotFound();
            }

            var turno = await _context.Turnos
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
            if (_context.Turnos == null)
            {
                return Problem("Entity set 'ApplicationDbContext.turnos'  is null.");
            }
            var turno = await _context.Turnos.FindAsync(id);
            if (turno != null)
            {
                _context.Turnos.Remove(turno);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool turnoExists(int id)
        {
          return (_context.Turnos?.Any(e => e.TurnoId == id)).GetValueOrDefault();
        }
    }
}
