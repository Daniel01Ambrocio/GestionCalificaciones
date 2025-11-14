using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gestionescolar.Entities
{
    public class Calificacion
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

        // Constructor vacío
        public Calificacion() { }

        // Constructor opcional con parámetros
        public Calificacion(int idAlumnoMateria, decimal? parcial1 = null, decimal? parcial2 = null,
                            decimal? parcial3 = null, decimal? parcial4 = null, decimal? promedio = null)
        {
            IDAlumnoMateria = idAlumnoMateria;
            Parcial1 = parcial1;
            Parcial2 = parcial2;
            Parcial3 = parcial3;
            Parcial4 = parcial4;
            Promedio = promedio;
        }
    }
}