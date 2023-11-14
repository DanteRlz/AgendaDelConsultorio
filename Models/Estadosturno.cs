using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AgendaDelConsultorio.Models
{
    public partial class estadosturno
    {
        public estadosturno()
        {
            turnos = new HashSet<turno>();
        }
        [Key]
        public int EstadoTurnoId { get; set; }
        [Display (Name = "Descripción")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Descripcion { get; set; } = null!;

        public virtual ICollection<turno>? turnos { get; set; }
    }
}
