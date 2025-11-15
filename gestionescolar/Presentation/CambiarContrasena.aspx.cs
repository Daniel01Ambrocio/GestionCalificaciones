using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace gestionescolar.Presentation
{
    public partial class CambiarContrasena : System.Web.UI.Page
    {
        private bool ValidarUsuario(string usuario, string status)
        {
            if (usuario != null && (status == "Activo" || status == "Egresado"))
            {
                return true;
            }
            else
            {
                return false;

            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Evitar caché del navegador
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetValidUntilExpires(false);

            // Verificar si hay sesión activa
            if (Session["Usuario"] == null)
            {
                Response.Redirect("index.aspx");   // tu página de login
            }
            if (!IsPostBack)
            {
                string usuario = Convert.ToString(Session["Usuario"]);
                string status = Convert.ToString(Session["Status"]);
                bool v = ValidarUsuario(usuario, status);
                if (v)
                {
                   
                }
                else
                {
                    Response.Redirect("index.aspx");
                }

            }
        }
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            string anterior = txtAnterior.Text;
            string nueva = txtNueva.Text;
            string confirmar = txtConfirmar.Text;

            if (nueva != confirmar)
            {
                lblMensaje.Text = "La nueva contraseña y la confirmación no coinciden.";
                lblMensaje.CssClass = "text-danger text-center";
                return;
            }

            // Aquí validar contraseña anterior y actualizar en BD
            bool actualizada = true; // ejemplo

            if (actualizada)
            {
                lblMensaje.Text = "Contraseña actualizada correctamente.";
                lblMensaje.CssClass = "text-success text-center";
            }
            else
            {
                lblMensaje.Text = "La contraseña anterior es incorrecta.";
                lblMensaje.CssClass = "text-danger text-center";
            }
        }

    }
}