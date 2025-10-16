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
            // Evitar que el navegador almacene la página en caché
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));

            if (!IsPostBack)
            {
                // Limpiar y abandonar la sesión al cargar el login
                Session.Clear();
                Session.Abandon();
            }
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
                switch (rol)
                {
                    case "Alumno":
                        // Redireccionar 
                        Response.Redirect("BienvenidaLogeado.aspx");
                        break;
                    case "Maestro":
                        // Redireccionar 
                        Response.Redirect("BienvenidaLogeado.aspx");
                        break;
                    case "Administrativo":
                        // Redireccionar 
                        Response.Redirect("BienvenidaLogeado.aspx");
                        break;
                    case "Director":
                        // Redireccionar 
                        Response.Redirect("BienvenidaLogeado.aspx");
                        break;



                }
                
            }
            else
            {
                MostrarAlerta("Usuario y/o contraseña incorrectos.", false);
               
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
        protected void MostrarAlerta(string mensaje, bool esExito)
        {
            // Color verde para éxito, rojo para error
            string color = esExito ? "green" : "red";

            // Script para mostrar una alerta centrada con estilos personalizados
            string script = $@"
                var alerta = document.createElement('div');
                alerta.innerText = '{mensaje}';
                alerta.style.position = 'fixed';
                alerta.style.top = '50%';
                alerta.style.left = '50%';
                alerta.style.transform = 'translate(-50%, -50%)';
                alerta.style.backgroundColor = '{color}';
                alerta.style.color = 'white';
                alerta.style.padding = '15px 30px';
                alerta.style.borderRadius = '8px';
                alerta.style.fontWeight = 'bold';
                alerta.style.boxShadow = '0 4px 12px rgba(0, 0, 0, 0.2)';
                alerta.style.zIndex = '9999';
                document.body.appendChild(alerta);
                setTimeout(function() {{ alerta.remove(); }}, 6000);";

            ScriptManager.RegisterStartupScript(this, GetType(), "mostrarAlerta", script, true);
        }
    }
}