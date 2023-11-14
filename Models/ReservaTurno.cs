using System.ComponentModel.DataAnnotations;

namespace AgendaDelConsultorio.Models
{
    public class ReservaTurno
    {
        [Display (Name = "Paciente")]
        public int PacienteId { get; set; }
        public paciente Paciente { get; set; }

        [Display (Name = "Fecha del Turno")]
        [Required (ErrorMessage = "El campo {0} es obligatorio")]
        [DataType (DataType.Date)]
        public DateTime FechaTurno { get; set; }

        [Display (Name = "Hora del Turno")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public TimeSpan HoraTurno { get; set; }

        [Display(Name = "Tipo de Especialidad")]
        public int TipoEspecialidadId { get; set; }

        [Display(Name = "Tipo de Especialidad")]

        public string TipoEspecialidadDescripcion { get; set; }

        [Display(Name = "Especialidad")]

        public int EspecialidadId { get; set; }

        [Display(Name = "Especialidad")]
        public string EspecialidadDescripcion { get; set; }

        [Display (Name = "Observación")]
        [DataType (DataType.MultilineText)]
        public string? Observacion { get; set; }
    }
}
