using gestionescolar.DLL;
using gestionescolar.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gestionescolar.BLL
{
    public class alumnoMateriaBLL
    {
        AlumnoMateriaDLL alumnoMateriaDLL = new AlumnoMateriaDLL();
        public string RegistrarAlumnoMateria(List<int> listaIdMateria, Entalumno entalumno)
        {
            return alumnoMateriaDLL.RegistrarAlumnoMateria(listaIdMateria, entalumno);
        }
    }
}