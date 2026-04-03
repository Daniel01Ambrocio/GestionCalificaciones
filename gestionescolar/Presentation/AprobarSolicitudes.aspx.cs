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
    public partial class AprobarSolicitudes : System.Web.UI.Page
    {
        DirectorBLL directorBLL = new DirectorBLL();
        SolicitudBajaBLL solicitudbajabll = new SolicitudBajaBLL();
        Entdirector entdirector = new Entdirector();
        UsuarioBLL usuarioBLL = new UsuarioBLL();
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
                    entdirector = directorBLL.ObtenerIDDirector(usuario);

                    if (entdirector.Iddirector < 1)
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
                        Session["IDDirector"] = entdirector.Iddirector;

                        MostrarSolicitudesPendientes();
                        MostrarSolicitudesAprobadas();
                    }


                }
                else
                {
                    Response.Redirect("Login.aspx");
                }

            }
        }
        public void MostrarSolicitudesPendientes()
        {
            DataTable dtSolicitudes = new DataTable();
            dtSolicitudes = solicitudbajabll.MostrarSolicitudesPendientes();
            gvSolicitudesPendientes.DataSource = dtSolicitudes;
            gvSolicitudesPendientes.DataBind();
        }
        public void MostrarSolicitudesAprobadas()
        {
            DataTable dtSolicitudes = new DataTable();
            dtSolicitudes = solicitudbajabll.MostrarSolicitudesAprobadas();
            gvHistorialAprovaciones.DataSource = dtSolicitudes;
            gvHistorialAprovaciones.DataBind();
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
        protected void gvSolicitudesPendientes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Aprobar")
            {
                int IDSolicitudBajas = Convert.ToInt32(e.CommandArgument);

                string mensaje = solicitudbajabll.AprobarSolicitud(IDSolicitudBajas);

                if (mensaje == "Actualización correcta.")
                {
                    //Cambiamos el status de la cuenta del usuario a Inectivo
                    bool cambioStatus = usuarioBLL.ActualizarStatusUsuario(IDSolicitudBajas);
                    if (cambioStatus)
                    {
                        MostrarAlerta(mensaje, true);
                    }
                    else
                    {
                        MostrarAlerta("Error al intentar desactivar la cuenta del usuario. Intentelo más tarde.", false);
                    }
                }
                else
                {
                    MostrarAlerta(mensaje, false);
                }

                MostrarSolicitudesPendientes();
                MostrarSolicitudesAprobadas();
            }
        }
        protected void btnFiltrarPendientes_Click(object sender, EventArgs e)
        {
            string textoBusqueda = txtFiltro.Text.Trim(); 
            DataTable dtOriginal = solicitudbajabll.MostrarSolicitudesPendientes();

            if (!string.IsNullOrEmpty(textoBusqueda) && dtOriginal.Rows.Count > 0)
            {
                string[] palabras = textoBusqueda.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                List<string> condiciones = new List<string>();

                // Recorrer todas las columnas del DataTable
                foreach (string palabra in palabras)
                {
                    string palabraLimpia = palabra.Replace("'", "''");
                    List<string> condicionesPorPalabra = new List<string>();

                    foreach (DataColumn col in dtOriginal.Columns)
                    {
                        // Solo filtrar columnas de tipo string o convertible a string
                        condicionesPorPalabra.Add($"ISNULL(CONVERT([{col.ColumnName}], 'System.String'),'') LIKE '%{palabraLimpia}%'");
                    }

                    // Combinar condiciones de todas las columnas con OR (la palabra puede estar en cualquiera)
                    condiciones.Add("(" + string.Join(" OR ", condicionesPorPalabra) + ")");
                }

                // Combinar condiciones de todas las palabras con AND (todas las palabras deben aparecer)
                string filtroFinal = string.Join(" AND ", condiciones);

                DataRow[] filasFiltradas = dtOriginal.Select(filtroFinal);

                gvSolicitudesPendientes.DataSource = filasFiltradas.Length > 0 ? filasFiltradas.CopyToDataTable() : null;
            }
            else
            {
                gvSolicitudesPendientes.DataSource = dtOriginal;
            }

            gvSolicitudesPendientes.DataBind();
        }

        protected void btnFiltrarHistorial_Click(object sender, EventArgs e)
        {
            string textoBusqueda = txtFiltrarAprobados.Text.Trim(); 
            DataTable dtOriginal = solicitudbajabll.MostrarSolicitudesAprobadas();

            if (!string.IsNullOrEmpty(textoBusqueda) && dtOriginal.Rows.Count > 0)
            {
                string[] palabras = textoBusqueda.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                List<string> condiciones = new List<string>();

                // Recorrer todas las columnas del DataTable
                foreach (string palabra in palabras)
                {
                    string palabraLimpia = palabra.Replace("'", "''");
                    List<string> condicionesPorPalabra = new List<string>();

                    foreach (DataColumn col in dtOriginal.Columns)
                    {
                        // Solo filtrar columnas de tipo string o convertible a string
                        condicionesPorPalabra.Add($"ISNULL(CONVERT([{col.ColumnName}], 'System.String'),'') LIKE '%{palabraLimpia}%'");
                    }

                    // Combinar condiciones de todas las columnas con OR (la palabra puede estar en cualquiera)
                    condiciones.Add("(" + string.Join(" OR ", condicionesPorPalabra) + ")");
                }

                // Combinar condiciones de todas las palabras con AND (todas las palabras deben aparecer)
                string filtroFinal = string.Join(" AND ", condiciones);

                DataRow[] filasFiltradas = dtOriginal.Select(filtroFinal);

                gvHistorialAprovaciones.DataSource = filasFiltradas.Length > 0 ? filasFiltradas.CopyToDataTable() : null;
            }
            else
            {
                gvHistorialAprovaciones.DataSource = dtOriginal;
            }

            gvHistorialAprovaciones.DataBind();
        }
    }
}