using System;
using System.Collections.Generic;

namespace AgendaDelConsultorio.Models
{
    public partial class Localidad
    {
        public Localidad()
        {
            pacientes = new HashSet<paciente>();
            profesionales = new HashSet<Profesional>();
        }

        public int Localidadid { get; set; }
        public int ProvinciaId { get; set; }
        public string Descripcion { get; set; } = null!;

        public virtual provincia Provincia { get; set; } = null!;
        public virtual ICollection<paciente> pacientes { get; set; }
        public virtual ICollection<Profesional> profesionales { get; set; }
    }
}
