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
            if ((usuario != null && status=="Activo")  || status != null)
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
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            Response.Expires = -1;
            if (Session["Usuario"] == null)
            {
                // Redirigir al login si no hay sesión
                Response.Redirect("index.aspx");
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
                    verCalificacionesGrupo.Visible = false;
                    solicitarBaja.Visible = false;
                    imprimirBoleta.Visible = false;
                    autorizarBajas.Visible = false;
                    bajaAdministrativos.Visible = false;
                    listaGrupos.Visible = false;
                    listaAlumnos.Visible = false;
                    listaMaestros.Visible = false;
                    listaAdministrativos.Visible = false;
                    registrarMateria.Visible = false;
                    ListaDirectores.Visible = false;

                    switch (rol)
                    {
                        case "Alumno":
                            misCalificaciones.Visible = true;
                            break;

                        case "Maestro":
                            asignarCalificacion.Visible = true;
                            verCalificacionesGrupo.Visible = true;
                            listaGrupos.Visible = true;
                            listaAlumnos.Visible = true;
                            break;

                        case "Administrativo":
                            registroUsuarios.Visible = true;
                            solicitarBaja.Visible = true;
                            imprimirBoleta.Visible = true;
                            listaGrupos.Visible = true;
                            listaAlumnos.Visible = true;
                            listaMaestros.Visible = true;
                            listaAdministrativos.Visible = true;
                            ListaDirectores.Visible = true;
                            registrarMateria.Visible = true;
                            break;

                        case "Directivo":
                            // Ver todo
                            registroUsuarios.Visible = true;
                            misCalificaciones.Visible = true;
                            asignarCalificacion.Visible = true;
                            verCalificacionesGrupo.Visible = true; 
                            solicitarBaja.Visible = true;
                            imprimirBoleta.Visible = true;
                            autorizarBajas.Visible = true;
                            bajaAdministrativos.Visible = true;
                            listaGrupos.Visible = true;
                            listaAlumnos.Visible = true;
                            listaMaestros.Visible = true;
                            listaAdministrativos.Visible = true;
                            ListaDirectores.Visible = true;
                            break;
                    }
                }
                else
                {
                    Response.Redirect("index.aspx");
                }

            }
        }

        protected void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            // Cierra la sesión del usuario autenticado
            FormsAuthentication.SignOut();

            Session.Clear();
            Session.Abandon();

            // Evita que se use el historial para volver
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            Response.Cache.SetExpires(DateTime.UtcNow.AddSeconds(-1));

            HttpContext.Current.Response.Redirect("index.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
    }
}