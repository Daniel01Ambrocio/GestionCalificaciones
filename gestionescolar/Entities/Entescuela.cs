using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace gestionescolar.Entities
{
    public class Entescuela
    {
        public int IDEscuela { get; set; } 
        public string NombreEscuela { get; set; } 
        public string ClaveInstitucion { get; set; } 
        public string Direccion { get; set; } 
        public string Telefono { get; set; } 
        public string Logotipo { get; set; } 
        public string CicloEscolar { get; set; }
    }
}