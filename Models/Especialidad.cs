using System;
using System.Collections.Generic;

namespace AgendaDelConsultorio.Models
{
    public partial class Especialidad
    {
        public Especialidad()
        {
            turnos = new HashSet<turno>();
        }

        public int EspecialidadId { get; set; }
        public string Descripcion { get; set; } = null!;
        public decimal Precio { get; set; }
        public int Duracion { get; set; }
        public string? Observacion { get; set; }
        public int TipoEspecialidadId { get; set; }

        public virtual tiposespecialidad TipoEspecialidad { get; set; } = null!;
        public virtual ICollection<turno> turnos { get; set; }
    }
}
