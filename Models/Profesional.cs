using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AgendaDelConsultorio.Models
{
    public partial class Profesional
    {
        [Key]
        public int ProfesionalId { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Apellido { get; set; } = null!;
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Nombre { get; set; } = null!;
        [Display (Name = "Fecha de Nacimiento")]
        [DataType (DataType.Date)]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public DateTime? FechaNacimiento { get; set; }
        [Display (Name = "Tipo de Documento")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string TipoDocumento { get; set; } = null!;
        [Display (Name = "Número de Documento")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int NumeroDocumento { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Calle { get; set; } = null!;
        public int Altura { get; set; }
        [Display (Name = "Provincia")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int ProvinciaId { get; set; }
        [Display (Name = "Localidad")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int LocalidadId { get; set; }
        [Display (Name = "Código Postal")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int CodigoPostal { get; set; }
        [Display (Name = "Correo electrónico")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string CorreoElectronico { get; set; } = null!;
        [Display (Name = "Teléfono")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Telefono { get; set; } = null!;
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Especialidad { get; set; } = null!;

        public virtual Localidad? Localidad { get; set; } = null!;
        public virtual provincia? Provincia { get; set; } = null!;
    }
}
