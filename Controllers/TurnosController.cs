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
using Microsoft.Data.SqlClient;

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
			return RedirectToAction("Index", "Ingreso");
		}

		private List<tiposespecialidad> ObtenerTiposDeEspecialidad()
		{
			var tiposEspecialidades =
				(
				from t in _context.TiposEspecialidades
				orderby t.Descripcion
				select t
				).ToList();

			return tiposEspecialidades;
		}

		private List<Especialidad> ObtenerEspecialidadPorTipoEspecialidad(int tipoEspecialidadId)
		{
			var especialidades =
				(
				from s in _context.Especialidades
				where s.TipoEspecialidadId == tipoEspecialidadId
				orderby s.Descripcion
				select s
				).ToList();

			return especialidades;
		}

		[HttpGet]

		public async Task<JsonResult> ObtenerJsonEspecialidadPorTipoEspecialidad(int tipoEspecialidadId)
		{
			var especialidad =
				(
				from s in ObtenerEspecialidadPorTipoEspecialidad(tipoEspecialidadId)
				select new { s.EspecialidadId, s.Descripcion }
				).ToList();

			return Json(especialidad);
		}

		private paciente? ObtenerPacientePorId(int pacienteId)
		{
			var paciente = _context.Pacientes
				.Include(c => c.Localidad)
				.Include(c => c.Provincia)
				.FirstOrDefault(m => m.PacienteId == pacienteId);

			return paciente;
		}

		#region ObtenerFechasDisponiblesDeTurnosProximos60Dias
		public List<DateTime> ObtenerFechasDisponiblesDeTurnosProximos60Dias(DateTime fecha)
		{
			var fechaDesde = fecha.Date; // Calcula la fecha mínima (fecha dada)
			var fechaHasta = fecha.AddDays(60).Date; // Calcula la fecha máxima (60 días después de la fecha dada)

			var fechas = (from t in _context.Turnos
						  where t.FechaTurno.Date >= fechaDesde &&
								t.FechaTurno.Date < fechaHasta &&
								t.EstadoTurnoId == (int)EstadoTurnoEnum.Libre
						  group t by t.FechaTurno.Date into grouped
						  orderby grouped.Key
						  select grouped.Key).ToList();
			return fechas;
		}

		public List<DateTime> ObtenerFechasDisponiblesDeTurnosProximos60Dias(int año, int mes, int dia)
		{
			var fecha = new DateTime(año, mes, dia);
			return ObtenerFechasDisponiblesDeTurnosProximos60Dias(fecha);
		}

		public List<DateTime> ObtenerFechasDisponiblesDeTurnosProximos60Dias(string fecha)
		{
			var fechaConvertida = DateTime.ParseExact(fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture);
			return ObtenerFechasDisponiblesDeTurnosProximos60Dias(fechaConvertida);
		}

		[HttpGet]
		public JsonResult ObtenerJsonDeFechasDisponiblesFormateadasYYYYMMDDProximos60Dias(int año, int mes, int dia)
		{
			var fecha = new DateTime(año, mes, dia);
			var fechas = ObtenerFechasDisponiblesDeTurnosProximos60Dias(fecha);
			var fechasFormateadas = FormatearFechasAStringYYYYMMDD(fechas);

			return Json(fechasFormateadas);
		}
		#endregion

		#region ObtenerHorasDisponiblesDeTurnosPorFecha
		public List<TimeSpan> ObtenerHorasDisponiblesDeTurnosPorFecha(DateTime fecha)
		{
			var horas = (from t in _context.Turnos
						 where t.FechaTurno.Date == fecha.Date && t.EstadoTurnoId == (int)EstadoTurnoEnum.Libre
						 orderby t.HoraTurno
						 select t.HoraTurno).ToList();
			return horas;
		}

		public List<TimeSpan> ObtenerHorasDisponiblesDeTurnosPorFecha(int año, int mes, int dia)
		{
			var fecha = new DateTime(año, mes, dia);
			return ObtenerHorasDisponiblesDeTurnosPorFecha(fecha);
		}

		public List<TimeSpan> ObtenerHorasDisponiblesDeTurnosPorFecha(string fecha)
		{
			var fechaConvertida = DateTime.ParseExact(fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture);
			return ObtenerHorasDisponiblesDeTurnosPorFecha(fechaConvertida);
		}

		public List<string> ObtenerHorasDisponiblesDeTurnosFormateadasPorFecha(string fecha)
		{
			var fechaConvertida = DateTime.ParseExact(fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture);
			var horas = ObtenerHorasDisponiblesDeTurnosPorFecha(fechaConvertida);

			var horasFormateadas = FormatearHorasAString(horas);
			return horasFormateadas;
		}

		#endregion

		#region Formatear las fecha y horas a string
		private static List<string> FormatearFechasAStringDDMMYYYY(List<DateTime> listaFechas)
		{
			var fechasFormateadas =
				(
				from f in listaFechas
				select f.ToString("dd/MM/yyyy")
				).ToList();

			return fechasFormateadas;
		}

		private static List<string> FormatearFechasAStringYYYYMMDD(List<DateTime> listaFechas)
		{
			var fechasFormateadas =
				(
				from f in listaFechas
				select f.ToString("yyyy-MM-dd")
				).ToList();

			return fechasFormateadas;
		}

		private static List<string> FormatearHorasAString(List<TimeSpan> listaHoras)
		{
			var horasFormateadas =
				(
				from h in listaHoras
				select h.ToString(@"hh\:mm")
				).ToList();

			return horasFormateadas;
		}
		#endregion
		//public List<DateTime> ObtenerFechasLibresDelMesActual()
		//{
		//    var diaDeHoy = DateTime.Now.Date;
		//    var primerDiaMesVigente = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
		//    var ultimoDiaMesVigente = primerDiaMesVigente.AddMonths(1).AddDays(-1);

		//    const int estadoTurnoIdLibre = 1;
		//    var fechas = _context.Turnos
		//        .Where(f => f.FechaTurno.Date == diaDeHoy
		//                    && f.FechaTurno.Date < ultimoDiaMesVigente
		//                    && f.EstadoTurnoId == estadoTurnoIdLibre)
		//        .GroupBy(f => f.FechaTurno.Date)
		//        .OrderBy(g => g.Key)
		//        .Select(g => g.Key)
		//        .ToList();

		//    return fechas;
		//}

		//public List<String> ObtenerHorasLibresPorFecha(String fecha)
		//{
		//    var fechaTurno = DateTime.ParseExact(fecha,"dd/mm/yyyy",
		//        CultureInfo.InvariantCulture);
		//    const int estadoTurnoIdLibre = 1;

		//    var horas = from t in _context.Turnos
		//                where t.FechaTurno.Date == fechaTurno.Date
		//                                        && t.EstadoTurnoId == estadoTurnoIdLibre
		//                orderby t.HoraTurno
		//                select t.HoraTurno;

		//    return horas.ToList().Select(h => h.ToString(@"hh\:mm")).ToList();
		//}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ReservarTurno(ReservaTurno model)
		{
			// Buscar el turno por fecha, hora y estado
			var turnoExistente = await (from t in _context.Turnos
										where t.FechaTurno == model.FechaTurno
										   && t.HoraTurno == model.HoraTurno
										   && t.EstadoTurnoId == (int)EstadoTurnoEnum.Libre
										select t).FirstOrDefaultAsync();

			if (turnoExistente != null)
			{
				// Actualizar el turno con los datos del modelo
				turnoExistente.PacienteId = model.PacienteId;
				turnoExistente.FechaTurno = model.FechaTurno;
				turnoExistente.HoraTurno = model.HoraTurno;
				turnoExistente.EspecialidadId = model.EspecialidadId;
				turnoExistente.Observacion = model.Observacion;
				turnoExistente.EstadoTurnoId = (int)EstadoTurnoEnum.Pendiente;

				// Guardar los cambios en la base de datos
				_context.Update(turnoExistente);
				await _context.SaveChangesAsync();

				// Pasamos el modelo a la vista 'TurnoReservado'.
				ViewBag.HideHeader = true;
				return View("TurnoReservado", model);

			}
			return View(model);
		}

		[HttpGet]
		public IActionResult BuscarPaciente()
		{
			ViewBag.HideHeader = true;
			return View();
		}
		[HttpPost]
		public IActionResult BuscarPaciente(string numeroDocumento, string email)
		{
			var paciente = (from c in _context.Pacientes
							where c.NumeroDocumento.ToString() == numeroDocumento || c.CorreoElectronico == email
							select c).FirstOrDefault();

			ViewBag.HideHeader = true;

			if (paciente == null)
			{
				ViewBag.ErrorMessage = "Paciente no encontrado";
				return View();
			}

			return View(paciente); // Pasamos el cliente a la vista para mostrar los detalles
		}
		public IActionResult IniciarReservaTurno(int pacienteId)
		{
			paciente? paciente = ObtenerPacientePorId(pacienteId);

			// Obtener los Tipos de Servicio
			var tiposEspecialidad = ObtenerTiposDeEspecialidad();
			var tipoEspecialidadId = tiposEspecialidad.FirstOrDefault()?.TipoEspecialidadId ?? 0;

			// Obtener los Servicios que pertenecen al Tipo de Servicio especificado
			var especialidad = ObtenerEspecialidadPorTipoEspecialidad(tipoEspecialidadId);

			// Obtener las fechas de los turnos libres
			var fechaHoy = DateTime.Today;
			var listaFechas = ObtenerFechasDisponiblesDeTurnosProximos60Dias(fechaHoy);
			var fecha = listaFechas.FirstOrDefault();
			var listaFechasFormateadas = FormatearFechasAStringDDMMYYYY(listaFechas);

			// Obtener las horas de los turnos libres
			var listaHoras = ObtenerHorasDisponiblesDeTurnosPorFecha(fecha);
			var hora = listaHoras.FirstOrDefault();
			var listaHorasFormateadas = FormatearHorasAString(listaHoras);

			var reservaTurno = new ReservaTurno
			{
				FechaTurno = fecha,
				HoraTurno = hora,
				Paciente = paciente,
				PacienteId = pacienteId,
			};

			ViewData["SelectListTiposEspecialidades"] = new SelectList(tiposEspecialidad, "TipoEspecialidadId", "Descripcion");
			ViewData["SelectListEspecialidades"] = new SelectList(especialidad, "EspecialidadId", "Descripcion");
			ViewData["SelectListFechas"] = new SelectList(listaFechasFormateadas);
			ViewData["SelectListHoras"] = new SelectList(listaHorasFormateadas);

			ViewBag.HideHeader = true;
			return View(reservaTurno);
		}

		[HttpGet]
		public ActionResult GenerarTurnosLibres()
		{
			return View();
		}

		[HttpPost]
		public async Task<ActionResult> GenerarTurnosLibres(GenerarTurnosLibresModel model)
		{
			if (ModelState.IsValid)
			{
				try
				{
					var parametros = new List<SqlParameter>
			{
				new SqlParameter("@FechaTurnoDesde", model.FechaTurnoDesde),
				new SqlParameter("@FechaTurnoHasta", model.FechaTurnoHasta),
				new SqlParameter("@HoraTurnoDesde", model.HoraTurnoDesde),
				new SqlParameter("@HoraTurnoHasta", model.HoraTurnoHasta),
				new SqlParameter("@Intervalo", model.Intervalo),
				new SqlParameter("@Lunes", model.Lunes),
				new SqlParameter("@Martes", model.Martes),
				new SqlParameter("@Miercoles", model.Miercoles),
				new SqlParameter("@Jueves", model.Jueves),
				new SqlParameter("@Viernes", model.Viernes),
				new SqlParameter("@Sabado", model.Sabado),
				new SqlParameter("@Domingo", model.Domingo)
			};

					var result = await _context.Database.ExecuteSqlRawAsync("EXEC GenerarTurnosLibres @FechaTurnoDesde, @FechaTurnoHasta, @HoraTurnoDesde, @HoraTurnoHasta, @Intervalo, @Lunes, @Martes, @Miercoles, @Jueves, @Viernes, @Sabado, @Domingo", parametros.ToArray());

					// Mostrar mensaje de éxito
					ViewBag.Message = "Turnos generados exitosamente.";
				}
				catch (Exception ex)
				{
					// Manejar excepción si ocurre un error al ejecutar el procedimiento almacenado
					ModelState.AddModelError(string.Empty, "Ocurrió un error al generar los turnos: " + ex.Message);
				}
			}

			return View(model);
		}
	}
}
