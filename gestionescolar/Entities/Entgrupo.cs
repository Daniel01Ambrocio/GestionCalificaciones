using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gestionescolar.Entities
{
    public class Entgrupo
    {
        public int IDGrupo { get; set; }
        public int grado { get; set; }
        public string grupo{ get; set; }
        public string anio { get; set; }
    }
}