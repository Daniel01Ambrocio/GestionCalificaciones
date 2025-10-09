using gestionescolar.DLL;
using gestionescolar.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gestionescolar.BLL
{
    public class AlumnoBLL
    {
        AlumnoDLL alumnoDLL = new AlumnoDLL();
        public string RegistrarAlumno(Entalumno entalumno)
        {

            return alumnoDLL.RegistrarAlumno(entalumno);
        }
    }
}