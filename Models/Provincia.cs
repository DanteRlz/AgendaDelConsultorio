using System;
using System.Collections.Generic;

namespace AgendaDelConsultorio.Models
{
    public partial class provincia
    {
        public provincia()
        {
            localidades = new HashSet<Localidad>();
            pacientes = new HashSet<paciente>();
            profesionales = new HashSet<Profesional>();
        }

        public int ProvinciaId { get; set; }
        public string Descripcion { get; set; } = null!;

        public virtual ICollection<Localidad> localidades { get; set; }
        public virtual ICollection<paciente> pacientes { get; set; }
        public virtual ICollection<Profesional> profesionales { get; set; }
    }
}
