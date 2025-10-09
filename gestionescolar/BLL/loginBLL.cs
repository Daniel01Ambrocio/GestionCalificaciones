using gestionescolar.DLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gestionescolar.BLL
{

    public class loginBLL
    {
        loginDLL loginDLL = new loginDLL();
        public (int idUsuario, string descripcionStatus, string nombreRol, string tipoUsuario)? InicioSesion(string user, string pass)
        {
            return loginDLL.InicioSesion(user, pass);
        }
    }
}