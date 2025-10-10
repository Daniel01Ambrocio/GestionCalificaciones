using gestionescolar.DLL;
using gestionescolar.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace gestionescolar.BLL
{
    public class MaestroBLL
    {
        MaestroDLL maestroDLL = new MaestroDLL();

        UsuarioBLL usuarioBLL = new UsuarioBLL();
        public string RegistrarMaestro(EntUsuario entUsuario, Entmaestro entmaestro)
        {

            entUsuario.IdUsuario = usuarioBLL.RegistrarUsuario(entUsuario);
            string msg = maestroDLL.RegistrarMaestro(entUsuario, entmaestro);
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
        public DataTable ObtenerMaestros()
        {
            return maestroDLL.ObtenerMaestros();
        }
    }
}