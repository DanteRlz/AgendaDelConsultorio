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
    public class EstadosTurnosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EstadosTurnosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: EstadosTurnos
        public async Task<IActionResult> Index()
        {
              return _context.EstadosTurnos != null ? 
                          View(await _context.EstadosTurnos.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.estadosturnos'  is null.");
        }

        // GET: EstadosTurnos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.EstadosTurnos == null)
            {
                return NotFound();
            }

            var estadosturno = await _context.EstadosTurnos
                .FirstOrDefaultAsync(m => m.EstadoTurnoId == id);
            if (estadosturno == null)
            {
                return NotFound();
            }

            return View(estadosturno);
        }

        // GET: EstadosTurnos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EstadosTurnos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EstadoTurnoId,Descripcion")] estadosturno estadosturno)
        {
            if (ModelState.IsValid)
            {
                _context.Add(estadosturno);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(estadosturno);
        }

        // GET: EstadosTurnos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.EstadosTurnos == null)
            {
                return NotFound();
            }

            var estadosturno = await _context.EstadosTurnos.FindAsync(id);
            if (estadosturno == null)
            {
                return NotFound();
            }
            return View(estadosturno);
        }

        // POST: EstadosTurnos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EstadoTurnoId,Descripcion")] estadosturno estadosturno)
        {
            if (id != estadosturno.EstadoTurnoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(estadosturno);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!estadosturnoExists(estadosturno.EstadoTurnoId))
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
            return View(estadosturno);
        }

        // GET: EstadosTurnos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.EstadosTurnos == null)
            {
                return NotFound();
            }

            var estadosturno = await _context.EstadosTurnos
                .FirstOrDefaultAsync(m => m.EstadoTurnoId == id);
            if (estadosturno == null)
            {
                return NotFound();
            }

            return View(estadosturno);
        }

        // POST: EstadosTurnos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.EstadosTurnos == null)
            {
                return Problem("Entity set 'ApplicationDbContext.estadosturnos'  is null.");
            }
            var estadosturno = await _context.EstadosTurnos.FindAsync(id);
            if (estadosturno != null)
            {
                _context.EstadosTurnos.Remove(estadosturno);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool estadosturnoExists(int id)
        {
          return (_context.EstadosTurnos?.Any(e => e.EstadoTurnoId == id)).GetValueOrDefault();
        }
    }
}
