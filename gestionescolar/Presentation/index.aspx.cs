using gestionescolar.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace gestionescolar.Presentation
{
    public partial class index : System.Web.UI.Page
    {
        loginBLL loginBLL = new loginBLL();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string usuario = txtUsuario.Text.Trim();
            string pass = HashPassword(txtPassword.Text.Trim()); // Suponiendo que ya tienes hashing implementado

            var resultado = loginBLL.InicioSesion(usuario, pass);

            if (resultado.HasValue)
            {
                // Obtener valores del resultado
                int id = resultado.Value.idUsuario;
                string status = resultado.Value.descripcionStatus;
                string rol = resultado.Value.nombreRol;
                string tipoUsuario = resultado.Value.tipoUsuario;

                // Aquí puedes guardar los datos en sesión o redirigir según rol, por ejemplo:
                Session["Usuario"]= txtUsuario.Text;
                Session["UsuarioID"] = id;
                Session["Rol"] = rol;
                Session["TipoUsuario"] = tipoUsuario;
                Session["Status"] = status;

                // Redireccionar 
                Response.Redirect("RegistroUsuarios.aspx");
            }
            else
            {
                // Mostrar mensaje de error (por ejemplo con un Label)
                //lblMensaje.Text = "Usuario o contraseña incorrectos.";
               
            }
        }

        //hashear contraseña
        private string HashPassword(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);

                // Convertir el hash a string hexadecimal
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x3"));
                }

                return sb.ToString();
            }
        }
    }
}