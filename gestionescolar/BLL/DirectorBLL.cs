using gestionescolar.DLL;
using gestionescolar.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gestionescolar.BLL
{
    public class DirectorBLL
    {
        DirectorDLL directorDLL = new DirectorDLL();
        public string RegistrarDirector(Entdirector entdirector)
        {

            return directorDLL.RegistrarDirector(entdirector);
        }
    }
}