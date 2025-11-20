using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gestionescolar.Entities
{
    public class Entcalificacion
    {
        public int IDCalificacion { get; set; }

        // Relación con AlumnoMateria
        public int IDAlumnoMateria { get; set; }

        // Parciales
        public decimal? Parcial1 { get; set; }
        public decimal? Parcial2 { get; set; }
        public decimal? Parcial3 { get; set; }
        public decimal? Parcial4 { get; set; }

        // Promedio
        public decimal? Promedio { get; set; }

        
    }
}