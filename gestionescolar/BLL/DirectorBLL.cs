using gestionescolar.DLL;
using gestionescolar.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace gestionescolar.BLL
{
    public class DirectorBLL
    {
        DirectorDLL directorDLL = new DirectorDLL();
        UsuarioBLL usuarioBLL = new UsuarioBLL();
        public string RegistrarDirector(EntUsuario entUsuario)
        {

            entUsuario.IdUsuario = usuarioBLL.RegistrarUsuario(entUsuario);
            string msg = directorDLL.RegistrarDirector(entUsuario);
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
        public DataTable ObtenerDirectores()
        {
            return directorDLL.ObtenerDirectores();
        }
    }
}