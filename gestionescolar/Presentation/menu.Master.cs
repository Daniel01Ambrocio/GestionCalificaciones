using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace gestionescolar.Presentation
{
    public partial class menu : System.Web.UI.MasterPage
    {
        private bool ValidarUsuario(string usuario, string status)
        {
            if (usuario != "0" && status.Equals("Activo", StringComparison.OrdinalIgnoreCase))
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
                    asignarGrupos.Visible = false;
                    solicitarBaja.Visible = false;
                    imprimirBoleta.Visible = false;
                    autorizarBajas.Visible = false;
                    bajaAdministrativos.Visible = false;
                    listaGrupos.Visible = false;
                    listaAlumnos.Visible = false;
                    listaMaestros.Visible = false;
                    listaAdministrativos.Visible = false;

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
                            asignarGrupos.Visible = true;
                            solicitarBaja.Visible = true;
                            imprimirBoleta.Visible = true;
                            listaGrupos.Visible = true;
                            listaAlumnos.Visible = true;
                            listaMaestros.Visible = true;
                            listaAdministrativos.Visible = true;
                            ListaDirectores.Visible = true;
                            break;

                        case "Directivo":
                            // Ver todo
                            registroUsuarios.Visible = true;
                            misCalificaciones.Visible = true;
                            asignarCalificacion.Visible = true;
                            verCalificacionesGrupo.Visible = true;
                            asignarGrupos.Visible = true;
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
        
    }
}