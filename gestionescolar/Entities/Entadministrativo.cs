using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gestionescolar.Entities
{
    public class Entadministrativo
    {
        public int IdAdministrativo { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string usuario { get; set; }
        public string contrasena { get; set; }
        public DateTime PeriodoIngreso { get; set; }
        public DateTime PeriodoFin { get; set; }
        public int IDStatus { get; set; }
        public int IDRol { get; set; }
    }
}