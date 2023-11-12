using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AgendaDelConsultorio.Models
{
    public partial class turno
    {
        [Key]
        public int TurnoId { get; set; }
        [Display(Name = "Fecha del Turno")]
        [DataType(DataType.Date)]
        public DateTime FechaTurno { get; set; }
        [Display(Name = "Hora del Turno")]
        public TimeSpan HoraTurno { get; set; }
        [Display(Name = "Estado del Turno")]
        public int EstadoTurnoId { get; set; }
        [Display(Name = "Paciente")]
        public int? PacienteId { get; set; }
        [Display(Name = "Especialidad")]
        public int? EspecialidadId { get; set; }
        [Display (Name = "Observación")]
        public string? Observacion { get; set; }

        public virtual Especialidad? Especialidad { get; set; }
        public virtual estadosturno? EstadoTurno { get; set; } = null!;
        public virtual paciente? Paciente { get; set; }
    }
}
