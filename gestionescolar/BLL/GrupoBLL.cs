using gestionescolar.DLL;
using gestionescolar.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace gestionescolar.BLL
{
    public class GrupoBLL
    {
        GrupoDLL grupoDLL = new GrupoDLL();
        public string RegistrarGrupo(Entgrupo grupo)
        {
            return grupoDLL.RegistrarGrupo(grupo);
        }
        public DataTable ObtenerGrupos()
        {
            return grupoDLL.ObtenerGrupos();
        }
        public DataTable ObtenerGruposConID()
        {
            return grupoDLL.ObtenerGruposConID();
        }
    }
}