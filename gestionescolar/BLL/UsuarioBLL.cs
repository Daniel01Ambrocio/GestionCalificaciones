using gestionescolar.DLL;
using gestionescolar.Entities;
using System;
using System.Collections.Generic;
using System.Data;
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
        public bool ConfirmaAnteriorContrasena(EntUsuario entUsuario)
        {
            return usuarioDLL.ConfirmaAnteriorContrasena(entUsuario);
        }
        public bool ActualizarContrasena(EntUsuario entUsuario)
        {
            return usuarioDLL.ActualizarContrasena(entUsuario);
        }
        public DataTable ObtenerUsuariosPorRol(int idRol)
        {
            return usuarioDLL.ObtenerUsuariosPorRol(idRol);
        }
        public bool ActualizarStatusUsuario(int IDSolicitudBajas)
        {
            return usuarioDLL.ActualizarStatusUsuario(IDSolicitudBajas);
        }
    }
}