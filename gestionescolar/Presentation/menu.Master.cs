using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace gestionescolar.Presentation
{
    public partial class menu : System.Web.UI.MasterPage
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
            Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            Response.Cache.SetNoServerCaching();

            // Verificar si hay sesión activa
            if (Session["Usuario"] == null)
            {
                Response.Redirect("Login.aspx");
            }
            if (!IsPostBack)
            {
                string usuario = Convert.ToString(Session["Usuario"]);
                string status = Convert.ToString(Session["Status"]);
                bool v = ValidarUsuario(usuario, status);
                if (v)
                {
                    lbUsuario.Text = "Usuario: " + Convert.ToString(Session["Usuario"]);
                    lbRol.Text = "Rol: " + Convert.ToString(Session["Rol"]);
                    string rol = Session["Rol"] as string;

                    // Ocultar todo por defecto
                    registroUsuarios.Visible = false;
                    misCalificaciones.Visible = false;
                    asignarCalificacion.Visible = false;
                    solicitarBaja.Visible = false;
                    imprimirBoleta.Visible = false;
                    autorizarBajas.Visible = false;
                    listaGrupos.Visible = false;
                    listaAlumnos.Visible = false;
                    listaMaestros.Visible = false;
                    listaAdministrativos.Visible = false;
                    registrarMateria.Visible = false;
                    ListaDirectoresV.Visible = false;
                    datosEscuela.Visible = false;
                    MisSolicitudes.Visible = false;
                    switch (rol)
                    {
                        case "Alumno":
                            misCalificaciones.Visible = true;
                            break;

                        case "Maestro":
                            asignarCalificacion.Visible = true;
                            break;

                        case "Administrativo":
                            registroUsuarios.Visible = true;
                            solicitarBaja.Visible = true;
                            imprimirBoleta.Visible = true;
                            listaGrupos.Visible = true;
                            listaAlumnos.Visible = true;
                            listaMaestros.Visible = true;
                            listaAdministrativos.Visible = true;
                            ListaDirectoresV.Visible = true;
                            registrarMateria.Visible = true;
                            datosEscuela.Visible = true;
                            MisSolicitudes.Visible = true;
                            break;

                        case "Director":
                            registroUsuarios.Visible = true;
                            imprimirBoleta.Visible = true;
                            autorizarBajas.Visible = true; 
                            listaGrupos.Visible = true;
                            listaAlumnos.Visible = true;
                            listaMaestros.Visible = true;
                            listaAdministrativos.Visible = true;
                            ListaDirectoresV.Visible = true;
                            datosEscuela.Visible = true;
                            break;
                    }
                }
                else
                {
                    Response.Redirect("login.aspx");
                }
            }
        }

        protected void btnCerrarSesion_Click(object sender, EventArgs e)
        {
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
    }
}