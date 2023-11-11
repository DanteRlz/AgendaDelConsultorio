using System;
using System.Collections.Generic;

namespace AgendaDelConsultorio.Models
{
    public partial class tiposespecialidad
    {
        public tiposespecialidad()
        {
            especialidades = new HashSet<Especialidad>();
        }

        public int TipoEspecialidadId { get; set; }
        public string Descripcion { get; set; } = null!;

        public virtual ICollection<Especialidad> especialidades { get; set; }
    }
}
