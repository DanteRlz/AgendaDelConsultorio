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
            var applicationDbContext = _context.Pacientes.Include(p => p.Localidad).Include(p => p.Provincia);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Pacientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Pacientes == null)
            {
                return NotFound();
            }

            var paciente = await _context.Pacientes
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
            ViewData["LocalidadId"] = new SelectList(_context.Localidades, "LocalidadId", "Descripcion");
            ViewData["ProvinciaId"] = new SelectList(_context.Provincias, "ProvinciaId", "Descripcion");
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
            ViewData["LocalidadId"] = new SelectList(_context.Localidades, "LocalidadId", "Descripcion", paciente.LocalidadId);
            ViewData["ProvinciaId"] = new SelectList(_context.Provincias, "ProvinciaId", "Descripcion", paciente.ProvinciaId);
            return View(paciente);
        }

        // GET: Pacientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Pacientes == null)
            {
                return NotFound();
            }

            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente == null)
            {
                return NotFound();
            }
            ViewData["LocalidadId"] = new SelectList(_context.Localidades, "LocalidadId", "Descripcion", paciente.LocalidadId);
            ViewData["ProvinciaId"] = new SelectList(_context.Provincias, "ProvinciaId", "Descripcion", paciente.ProvinciaId);
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
            ViewData["LocalidadId"] = new SelectList(_context.Localidades, "LocalidadId", "Descripcion", paciente.LocalidadId);
            ViewData["ProvinciaId"] = new SelectList(_context.Provincias, "ProvinciaId", "Descripcion", paciente.ProvinciaId);
            return View(paciente);
        }

        // GET: Pacientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Pacientes == null)
            {
                return NotFound();
            }

            var paciente = await _context.Pacientes
                .Include(p => p.Localidad)
                .Include(p => p.Provincia)
                .FirstOrDefaultAsync(m => m.PacienteId == id);
            if (paciente == null)
            {
                return NotFound();
            }

            return View(paciente);
        }
        [HttpGet]

        public async Task<IActionResult> ObtenerLocalidadesPorProvincia(int provinciaId)
        {
            var localidades = await _context.Localidades
                .Where(l => l.ProvinciaId == provinciaId)
                .Select(l => new {l.LocalidadId, l.Descripcion})
                .ToListAsync();

            return Json(localidades);
        }
        // POST: Pacientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Pacientes == null)
            {
                return Problem("Entity set 'ApplicationDbContext.pacientes'  is null.");
            }
            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente != null)
            {
                _context.Pacientes.Remove(paciente);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool pacienteExists(int id)
        {
          return (_context.Pacientes?.Any(e => e.PacienteId == id)).GetValueOrDefault();
        }

                // GET: Clientes/Registrar
        public IActionResult Registrar()
        {
            ViewData["LocalidadId"] = new SelectList(_context.Localidades, "LocalidadId", "Descripcion");
            ViewData["ProvinciaId"] = new SelectList(_context.Provincias, "ProvinciaId", "Descripcion");
            ViewBag.HideHeader = true;
            return View();
        }


        // POST: Clientes/Registrar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registrar([Bind("PacienteId,Apellido,Nombre,FechaNacimiento,TipoDocumento,NumeroDocumento,Calle,Altura,ProvinciaId,LocalidadId,CodigoPostal,CorreoElectronico,Telefono")] paciente paciente)
        {
            // En caso de error en la validación, repoblar las listas desplegables y mostrar el formulario nuevamente
            ViewData["LocalidadId"] = new SelectList(_context.Localidades, "LocalidadId", "Descripcion");
            ViewData["ProvinciaId"] = new SelectList(_context.Provincias, "ProvinciaId", "Descripcion");
            // Validar el modelo del paciente proporcionado
            if (ModelState.IsValid)
            {
                // Verificar si el número de documento o correo electrónico ya están registrados
                bool documentoExiste = (from c in _context.Pacientes
                                        where c.NumeroDocumento == paciente.NumeroDocumento
                                        select c).Any();

                bool correoExiste = (from c in _context.Pacientes
                                     where c.CorreoElectronico == paciente.CorreoElectronico
                                     select c).Any();


                if (documentoExiste)
                {
                    ModelState.AddModelError("NumeroDocumento", "El número de documento ya está registrado en la base de datos.");
                }

                if (correoExiste)
                {
                    ModelState.AddModelError("CorreoElectronico", "El correo electrónico ya está registrado en la base de datos.");
                }

                if (!documentoExiste && !correoExiste)
                {

                    // Agregar el cliente al contexto y guardar cambios
                    _context.Add(paciente);
                    await _context.SaveChangesAsync();

                    // Redirigir al Index después de crear exitosamente
                    return RedirectToAction(nameof(RegistroExitoso));
                }
            }

          

            ViewBag.HideHeader = true;
            return View(paciente);
        }

        public IActionResult RegistroExitoso()
        {
            ViewBag.HideHeader = true;
            ViewBag.Message = "El registro se ha completado con éxito.";
            return View();
        }
    }
}

