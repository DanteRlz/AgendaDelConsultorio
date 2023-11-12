using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AgendaDelConsultorio.Models
{
    public partial class Profesional
    {
        [Key]
        public int ProfesionalId { get; set; }
        [Required]
        public string Apellido { get; set; } = null!;
        [Required]
        public string Nombre { get; set; } = null!;
        [Display (Name = "Fecha de Nacimiento")]
        [DataType (DataType.Date)]
        public DateTime? FechaNacimiento { get; set; }
        [Display (Name = "Tipo de Documento")]
        public string TipoDocumento { get; set; } = null!;
        [Display (Name = "Número de Documento")]
        [Required]
        public int NumeroDocumento { get; set; }
        [Required]
        public string Calle { get; set; } = null!;
        public int Altura { get; set; }
        [Display (Name = "Provincia")]
        public int ProvinciaId { get; set; }
        [Display (Name = "Localidad")]
        public int LocalidadId { get; set; }
        [Display (Name = "Código Postal")]
        public int CodigoPostal { get; set; }
        [Display (Name = "Correo electrónico")]
        [Required]
        public string CorreoElectronico { get; set; } = null!;
        [Display (Name = "Teléfono")]
        [Required]
        public string Telefono { get; set; } = null!;
        [Required]
        public string Especialidad { get; set; } = null!;

        public virtual Localidad? Localidad { get; set; } = null!;
        public virtual provincia? Provincia { get; set; } = null!;
    }
}
