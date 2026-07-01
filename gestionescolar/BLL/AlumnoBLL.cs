using gestionescolar.DLL;
using gestionescolar.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace gestionescolar.BLL
{
    public class AlumnoBLL
    {
        AlumnoDLL alumnoDLL = new AlumnoDLL();
        UsuarioBLL usuarioBLL = new UsuarioBLL();
        public string RegistrarAlumno(EntUsuario entUsuario, Entalumno entalumno)
        {

            entUsuario.IdUsuario = usuarioBLL.RegistrarUsuario(entUsuario);
            string msg = alumnoDLL.RegistrarAlumno(entUsuario, entalumno);
            if (msg == "Error de registro")
            {
                return msg;
            }
            else
            {
                entUsuario.usuario = msg;
                bool valida = usuarioBLL.ActualizaUser(entUsuario);
                if (valida)
                {
                    return "Registro exitoso.";
                }
                else
                {
                    return "Error de registro";
                }
            }
        }
        public string ObtenerAlumPorFullNamYGrupo(EntUsuario entUsuario, Entalumno entalumno)
        {
            return alumnoDLL.ObtenerAlumPorFullNamYGrupo(entUsuario, entalumno);
        }
        public string ActualizarGrupoAlumno(Entalumno alumnoent)
        {
            return alumnoDLL.ActualizarGrupoAlumno(alumnoent);
        }
        public int BuscarMatriculaByUsuario(EntUsuario entUsuario)
        {
            return alumnoDLL.BuscarMatriculaByUsuario(entUsuario);
        }
        public DataTable ObtenerAlumnos()
        {
            return alumnoDLL.ObtenerAlumnos();
        }
        public DataTable ObtenerAlumnosPorGrupo(Entgrupo entgrupo)
        {
            return alumnoDLL.ObtenerAlumnosPorGrupo(entgrupo);
        }
        public DataTable ObtenerAlumnoPorMatricula(Entalumno entalumno)
        {
            return alumnoDLL.ObtenerAlumnoPorMatricula(entalumno);
        }
        public int CantidadAlumnosEngrupo(Entgrupo entgrupo)
        {
            return alumnoDLL.CantidadAlumnosEngrupo(entgrupo);
        }
    }
}