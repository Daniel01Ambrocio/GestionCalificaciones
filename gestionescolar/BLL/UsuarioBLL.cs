using gestionescolar.DLL;
using gestionescolar.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gestionescolar.BLL
{
    public class UsuarioBLL
    {
        UsuarioDLL usuarioDLL = new UsuarioDLL(); 
        public int RegistrarUsuario(EntUsuario entUsuario)
        {
            return usuarioDLL.RegistrarUsuario(entUsuario);
        }
        public bool ActualizaUser(EntUsuario entUsuario)
        {
            return usuarioDLL.ActualizaUser(entUsuario);
        }
    }
}