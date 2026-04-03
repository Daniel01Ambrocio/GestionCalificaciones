using gestionescolar.BLL;
using gestionescolar.DLL;
using gestionescolar.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace gestionescolar.Presentation
{
    public partial class SolicitudBaja : System.Web.UI.Page
    {
        RolBLL rolBLL = new RolBLL();
        UsuarioBLL UsuarioBLL = new UsuarioBLL();
        DirectorBLL directorBLL = new DirectorBLL();
        SolicitudBajaBLL solicitudbajabll = new SolicitudBajaBLL();
        EntSolicitudBajas entsolicitudBajas = new EntSolicitudBajas();
        AdministrativoBLL AdministrativoBLL = new AdministrativoBLL();
        Entadministrativo entadministrativo = new Entadministrativo();

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
                Response.Redirect("Login.aspx");
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
                    usuario = Session["Usuario"].ToString();
                    entadministrativo= AdministrativoBLL.ObtenerIDAdministrativo(usuario);

                    if (entadministrativo.IdAdministrativo < 1)
                    {
                        MostrarAlerta("Ocurrio un error, inicie sesión nuevamente.", false);
                        // 1. Cerrar sesión de Forms Authentication
                        FormsAuthentication.SignOut();

                        // 2. Abandonar la sesión actual
                        Session.Clear();
                        Session.Abandon();

                        // 3. Eliminar cookies de sesión y autenticación
                        if (Request.Cookies["ASP.NET_SessionId"] != null)
                        {
                            Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddDays(-1);
                        }
                        if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
                        {
                            Response.Cookies[FormsAuthentication.FormsCookieName].Expires = DateTime.Now.AddDays(-1);
                        }

                        // 4. Evitar que el navegador use caché para volver atrás
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        Response.Cache.SetNoStore();
                        Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));

                        // 5. Redirigir al login
                        Response.Redirect("Login.aspx");
                    }
                    else
                    {
                        Session["IdAdministrativo"] = entadministrativo.IdAdministrativo;
                        LimpiarFormulario();
                        CargarRoles();
                        CargarDirectoresActivos();
                    }
                        
                }
                else
                {
                    Response.Redirect("Login.aspx");
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
        private void CargarDirectoresActivos()
        {
            DataTable dtDirectores = new DataTable();
            ddlDirectivoAprov.Items.Clear(); // Limpiar antes de cargar
            dtDirectores = directorBLL.ObtenerDirectoresActivos();
            ddlDirectivoAprov.DataSource = dtDirectores;
            ddlDirectivoAprov.DataTextField = "NombreDirector";
            ddlDirectivoAprov.DataValueField = "Iddirector";
            ddlDirectivoAprov.DataBind();

            // Agrega el ítem por defecto
            ddlDirectivoAprov.Items.Insert(0, new ListItem("Selecciona un directivo", ""));
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
          

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }
        private void LimpiarFormulario()
        {
            // Deseleccionar
            ddlRol.ClearSelection();

            // Borrar todos los elementos
            ddlUsuario.Items.Clear();

            // Limpiar textbox
            txtMotivo.Text = string.Empty;

            // Deseleccionar
            ddlDirectivoAprov.ClearSelection();
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

        protected void btnEnviar_Click(object sender, EventArgs e)
        {
            // Validamos que los campos estén llenos
            if (string.IsNullOrEmpty(ddlRol.SelectedValue))
            {
                MostrarAlerta("Debe de seleccionar un rol para el usuario que desea suspender.", false);
                return;
            }

            if (string.IsNullOrEmpty(ddlUsuario.SelectedValue))
            {
                MostrarAlerta("Debe de seleccionar un usuario que desea suspender.", false);
                return;
            }

            if (string.IsNullOrEmpty(ddlDirectivoAprov.SelectedValue))
            {
                MostrarAlerta("Debe de seleccionar un directivo para aprobar su solicitud.", false);
                return;
            }

            if (string.IsNullOrEmpty(txtMotivo.Text))
            {
                MostrarAlerta("Debe de ingresar un motivo.", false);
                return;
            }
            
            entsolicitudBajas.IDAdministrativo = Convert.ToInt16(Session["IdAdministrativo"]);
            entsolicitudBajas.IDDirectivo = Convert.ToInt16(ddlDirectivoAprov.SelectedValue);
            entsolicitudBajas.IDUsuarioBaja = Convert.ToInt16(ddlUsuario.SelectedValue);
            entsolicitudBajas.Descripcion = txtMotivo.Text;
            entsolicitudBajas.FechaSolicitud = DateTime.Today;
            entsolicitudBajas.Estado = "Pendiente";

            // Todos los campos estan llenos:
            string mensaje = solicitudbajabll.RegistrarSolicitud(entsolicitudBajas);
            if(mensaje == "Registro correcto.")
            {
                MostrarAlerta(mensaje, true);
                LimpiarFormulario();
            }
            else
            {
                MostrarAlerta(mensaje, false);
            }
        }
    }
}