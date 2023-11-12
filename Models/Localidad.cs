using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AgendaDelConsultorio.Models
{
    public partial class Localidad
    {
        public Localidad()
        {
            pacientes = new HashSet<paciente>();
            profesionales = new HashSet<Profesional>();
        }
        [Key]
        public int Localidadid { get; set; }
        [Display (Name = "Provincia")]
        public int ProvinciaId { get; set; }
        [Display (Name = "Descripción")]
        [Required]
        public string Descripcion { get; set; } = null!;

        public virtual provincia? Provincia { get; set; } = null!;
        public virtual ICollection<paciente>? pacientes { get; set; }
        public virtual ICollection<Profesional>? profesionales { get; set; }
    }
}
