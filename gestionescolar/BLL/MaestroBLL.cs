using gestionescolar.DLL;
using gestionescolar.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gestionescolar.BLL
{
    public class MaestroBLL
    {
        MaestroDLL maestroDLL = new MaestroDLL();
        public string RegistrarMaestro(Entmaestro entmaestro)
        {

            return maestroDLL.RegistrarMaestro(entmaestro);
        }
    }
}