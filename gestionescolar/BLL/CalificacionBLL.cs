using gestionescolar.DLL;
using gestionescolar.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace gestionescolar.BLL
{
    public class CalificacionBLL
    {
        CalificacionDLL calificacionDLL = new CalificacionDLL();
        public DataTable MostrarCalificaciones(Entalumno entalumno)
        {
            return calificacionDLL.MostrarCalificaciones(entalumno);
        }
        public DataTable MostrarAlumnosCalificaciones(Entgrupo entgrupo)
        {
            return calificacionDLL.MostrarAlumnosCalificaciones(entgrupo);
        }
        public bool ActualizarCalificaciones(Entcalificacion entcalificacion)
        {
            return calificacionDLL.ActualizarCalificaciones(entcalificacion);
        }
    }
}