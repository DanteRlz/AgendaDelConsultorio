using System;
using System.Collections.Generic;

namespace AgendaDelConsultorio.Models
{
    public partial class turno
    {
        public int TurnoId { get; set; }
        public DateTime FechaTurno { get; set; }
        public TimeSpan HoraTurno { get; set; }
        public int EstadoTurnoId { get; set; }
        public int? PacienteId { get; set; }
        public int? EspecialidadId { get; set; }
        public string? Observacion { get; set; }

        public virtual Especialidad? Especialidad { get; set; }
        public virtual estadosturno EstadoTurno { get; set; } = null!;
        public virtual paciente? Paciente { get; set; }
    }
}
