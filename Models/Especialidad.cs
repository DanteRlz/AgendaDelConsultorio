using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AgendaDelConsultorio.Models
{
    public partial class Especialidad
    {
        public Especialidad()
        {
            turnos = new HashSet<turno>();
        }
        [Key]
        public int EspecialidadId { get; set; }
        [Display (Name = "Descripción")]
        [Required]
        public string Descripcion { get; set; } = null!;
        [Required]
        public decimal Precio { get; set; }
        [Display (Name = "Duración")]
        [Required]
        public int Duracion { get; set; }
        [Display (Name = "Observación")]
        public string? Observacion { get; set; }
        [Display (Name = "Tipo de especialidad")]
        [Required]
        public int TipoEspecialidadId { get; set; }

        public virtual tiposespecialidad? TipoEspecialidad { get; set; } = null!;
        public virtual ICollection<turno>? turnos { get; set; }
    }
}
