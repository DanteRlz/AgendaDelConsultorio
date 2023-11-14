using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AgendaDelConsultorio.Models
{
    public partial class tiposespecialidad
    {
        public tiposespecialidad()
        {
            especialidades = new HashSet<Especialidad>();
        }
        [Key]
        public int TipoEspecialidadId { get; set; }
        [Display (Name = "Descripción")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Descripcion { get; set; } = null!;

        public virtual ICollection<Especialidad>? especialidades { get; set; }
    }
}
