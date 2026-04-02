using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gestionescolar.Entities
{
    public class EntSolicitudBajas
    {
        public int IDSolicitudBajas { get; set; }
        public int IDAdministrativo { get; set; }
        public int IDDirectivo { get; set; }
        public int IDUsuarioBaja { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public DateTime FechaAprobacion { get; set; }
        public string Estado { get; set; }
    }
}