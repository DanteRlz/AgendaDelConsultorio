using System;
using System.Collections.Generic;

namespace AgendaDelConsultorio.Models
{
    public partial class Profesional
    {
        public int ProfesionalId { get; set; }
        public string Apellido { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public DateTime? FechaNacimiento { get; set; }
        public string TipoDocumento { get; set; } = null!;
        public int NumeroDocumento { get; set; }
        public string Calle { get; set; } = null!;
        public int Altura { get; set; }
        public int ProvinciaId { get; set; }
        public int LocalidadId { get; set; }
        public int CodigoPostal { get; set; }
        public string CorreoElectronico { get; set; } = null!;
        public string Telefono { get; set; } = null!;
        public string Especialidad { get; set; } = null!;

        public virtual Localidad Localidad { get; set; } = null!;
        public virtual provincia Provincia { get; set; } = null!;
    }
}
