using gestionescolar.DLL;
using gestionescolar.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gestionescolar.BLL
{
    public class EscuelaBLL
    {
        EscuelaDLL escuelaDLL = new EscuelaDLL();
        public Entescuela ObtenerEscuela()
        {
            return escuelaDLL.ObtenerEscuela();
        }
        public string ActualizarEscuela(Entescuela escuela)
        {
            return escuelaDLL.ActualizarEscuela(escuela);
        }
    }
}