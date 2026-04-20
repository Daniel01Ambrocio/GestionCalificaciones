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
    public partial class FinInicioPeriodo : System.Web.UI.Page
    { 
        AdministrativoBLL AdministrativoBLL = new AdministrativoBLL();
        Entadministrativo entadministrativo = new Entadministrativo();
        GrupoBLL grupoBLL = new GrupoBLL();
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
                    entadministrativo = AdministrativoBLL.ObtenerIDAdministrativo(usuario);

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
                        //logica para el usuario logueado correctamente
                        int periodoActual = 0;
                        int periodoNuevo = 0;
                        periodoActual = (DateTime.Now.Year)-1;
                        periodoNuevo = DateTime.Now.Year;
                        btnTerminarComenzar.Text = "Terminar periodo " + periodoActual + " comenzar periodo " + periodoNuevo;
                        MostrarGruposDelPeriodo();
                    }

                }
                else
                {
                    Response.Redirect("Login.aspx");
                }

            }
        }
        public void MostrarGruposDelPeriodo()
        {
            int periodoActual = 0;
            periodoActual = (DateTime.Now.Year) - 1;
            DataTable dataGrupos = new DataTable();
            dataGrupos = grupoBLL.ObtenerGruposDelPeriodo(periodoActual);
            gvGrupos.DataSource = dataGrupos;
            gvGrupos.DataBind();
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

        protected void btnTerminarComenzar_Click(object sender, EventArgs e)
        {
            int periodoActual = 0;
            int periodoNuevo = 0;
            periodoActual = (DateTime.Now.Year) - 1;
            periodoNuevo = DateTime.Now.Year;
            string mensaje = "";
            mensaje= grupoBLL.GenerarGruposNuevoPeriodo(periodoActual, periodoNuevo);

        }
    }
}