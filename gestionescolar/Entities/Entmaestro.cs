using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gestionescolar.Entities
{
    public class Entmaestro
    {
        public int IdMaestro { get; set; }
        public int IDGrupo { get; set; }
        public string cedulaprofesional { get; set; }
        public int IDUsuario { get; set; }
    }
}