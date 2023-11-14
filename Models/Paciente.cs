
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AgendaDelConsultorio.Models
{
    public partial class paciente
    {
        public paciente()
        {
            turnos = new HashSet<turno>();
        }
        [Key]
        public int PacienteId { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Apellido { get; set; } = null!;
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Nombre { get; set; } = null!;
        [Display (Name ="Fecha de Nacimiento") ]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public DateTime? FechaNacimiento { get; set; }
        [Display (Name ="Tipo de Documento")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string TipoDocumento { get; set; } = null!;
        [Display (Name = "Número de documento")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int NumeroDocumento { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Calle { get; set; } = null!;
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int Altura { get; set; }
        [Display (Name = "Provincia")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int ProvinciaId { get; set; }
        [Display (Name = "Localidad")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int LocalidadId { get; set; }
        [Display (Name = "Código Postal")]
        public int CodigoPostal { get; set; }
        [Display (Name = "Correo electrónico")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string CorreoElectronico { get; set; } = null!;
        [Display (Name = "Teléfono")]
        public string Telefono { get; set; } = null!;

        public virtual Localidad? Localidad { get; set; } = null!;
        public virtual provincia? Provincia { get; set; } = null!;
        public virtual ICollection<turno>? turnos { get; set; }
    }
}
