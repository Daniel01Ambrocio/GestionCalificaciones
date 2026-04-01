using gestionescolar.BLL;
using gestionescolar.DLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace gestionescolar.Presentation
{
    public partial class SolicitudBaja : System.Web.UI.Page
    {
        RolBLL rolBLL = new RolBLL();
        UsuarioBLL UsuarioBLL = new UsuarioBLL();
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
            string ms = Convert.ToString(Session["mensaje"]);
            if (ms.Length > 0)
            {
                MostrarAlerta(ms, true);
                Session["mensaje"] = "";
            }
            if (!IsPostBack)
            {
                string usuario = Convert.ToString(Session["Usuario"]);
                string status = Convert.ToString(Session["Status"]);
                bool v = ValidarUsuario(usuario, status);
                if (v)
                {
                    // Ocultar los divs al cargar la página
                    
                    CargarRoles();
                }
                else
                {
                    Response.Redirect("index.aspx");
                }

            }
        }
        private void CargarRoles()
        {
            DataTable dt = new DataTable();
            ddlRol.Items.Clear(); // Limpiar antes de cargar
            dt = rolBLL.ObtenerRoles();
            ddlRol.DataSource = dt;
            ddlRol.DataTextField = "NombreRol";
            ddlRol.DataValueField = "IdRol";
            ddlRol.DataBind();

            // Agrega el ítem por defecto
            ddlRol.Items.Insert(0, new ListItem("Selecciona un rol", ""));
        }
        
        private void CargarUsuarios(int idRol)
        {
            ddlUsuario.Items.Clear();

            if (idRol == 0)
            {
                ddlUsuario.Items.Insert(0, new ListItem("Seleccione un usuario", ""));
                return;
            }

            DataTable dt = UsuarioBLL.ObtenerUsuariosPorRol(idRol);

            ddlUsuario.DataSource = dt;
            ddlUsuario.DataTextField = "NombreUsuario"; // ajusta al nombre real
            ddlUsuario.DataValueField = "IdUsuario";
            ddlUsuario.DataBind();

            ddlUsuario.Items.Insert(0, new ListItem("Seleccione un usuario", ""));
        }
        protected void ddlRol_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idRol;

            if (int.TryParse(ddlRol.SelectedValue, out idRol))
            {
                CargarUsuarios(idRol);
            }
            else
            {
                // Si no selecciona nada, limpiar usuarios
                ddlUsuario.Items.Clear();
                ddlUsuario.Items.Insert(0, new ListItem("Seleccione un usuario", ""));
            }
        }
       

        protected void btnAtras_Click(object sender, EventArgs e)
        {

        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {

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