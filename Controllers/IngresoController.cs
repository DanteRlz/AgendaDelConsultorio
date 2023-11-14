using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AgendaDelConsultorio.Data;
using AgendaDelConsultorio.Models;


namespace AgendaServicios.Web.Controllers
{
	public class IngresoController : Controller
	{
		private readonly ApplicationDbContext _context;

		public IngresoController(ApplicationDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public IActionResult Index()
		{
			ViewBag.HideHeader = true;
			return View();
		}

		[HttpPost]
		public IActionResult Index(string numeroDocumento, string email)
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
			return RedirectToAction("IniciarReservaTurno", "Turnos", new { pacienteId = paciente.PacienteId });
		}

	}
}