using System;
using System.Collections.Generic;

namespace AgendaDelConsultorio.Models
{
    public partial class estadosturno
    {
        public estadosturno()
        {
            turnos = new HashSet<turno>();
        }

        public int EstadoTurnoId { get; set; }
        public string Descripcion { get; set; } = null!;

        public virtual ICollection<turno> turnos { get; set; }
    }
}
