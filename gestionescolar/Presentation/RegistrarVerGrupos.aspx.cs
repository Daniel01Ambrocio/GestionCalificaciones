using gestionescolar.BLL;
using gestionescolar.DLL;
using gestionescolar.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace gestionescolar.Presentation
{
    public partial class RegistrarVerGrupos : System.Web.UI.Page
    {
        GrupoBLL grupoBLL = new GrupoBLL();
        Entgrupo entgrupo = new Entgrupo();
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
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            Response.Expires = -1;
            if (Session["Usuario"] == null)
            {
                // Redirigir al login si no hay sesión
                Response.Redirect("index.aspx");
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
                    LimpiarFormulario();
                    MostrarGrupos();
                }
                else
                {
                    Response.Redirect("index.aspx");
                }

            }
        } 
        public void MostrarGrupos()
        {
            DataTable dataGrupos = new DataTable();
            dataGrupos = grupoBLL.ObtenerGrupos();
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

        protected void btnRegistrar_Click(object sender, EventArgs e)
        {
            string mensaje = "";
            if (string.IsNullOrWhiteSpace(txtAnio.Text) ||
                string.IsNullOrWhiteSpace(txtGrado.Text) ||
                string.IsNullOrWhiteSpace(txtGrupo.Text))
            {
                MostrarAlerta("Todos los campos deben estar llenos.", false);
            }
            else
            {
                entgrupo.grupo = txtGrupo.Text;
                entgrupo.grado=Convert.ToInt16(txtGrado.Text);
                entgrupo.anio = Convert.ToInt16(txtAnio.Text);
                mensaje = grupoBLL.RegistrarGrupo(entgrupo);
                if (mensaje == "Registro exitoso.")
                {
                    LimpiarFormulario();
                    MostrarGrupos();
                    Session["mensaje"] = mensaje;
                    Response.Redirect(Request.RawUrl);
                }
                else
                {
                    MostrarAlerta(mensaje, false);
                }
            }

            entgrupo = new Entgrupo();
        }
        protected void gvGrupos_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string mensaje = "";
            entgrupo.IDGrupo = Convert.ToInt32(gvGrupos.DataKeys[e.RowIndex].Value);
            mensaje = grupoBLL.EliminarGrupo(entgrupo);
            if (mensaje == "Eliminación exitosa.")
            {
                LimpiarFormulario();
                MostrarGrupos();
                Session["mensaje"] = mensaje;
                Response.Redirect(Request.RawUrl);
            }
            else
            {
                MostrarAlerta(mensaje, false);
            }
        }

        private void LimpiarFormulario()
        {
            txtGrado.Text = "";
            txtGrupo.Text = "";
            txtAnio.Text = "";
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            string textoBusqueda = txtFiltro.Text.Trim();

            DataTable dtOriginal = grupoBLL.ObtenerGrupos(); // Tu método para cargar los grupos

            if (!string.IsNullOrEmpty(textoBusqueda))
            {
                // Separar por palabras
                string[] palabras = textoBusqueda.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                // Armar condiciones por cada palabra (en grado, Grupo, anio)
                List<string> condiciones = new List<string>();
                foreach (string palabra in palabras)
                {
                    condiciones.Add($"(CONVERT(Grado, System.String) LIKE '%{palabra}%' OR Grupo LIKE '%{palabra}%' OR CONVERT(anio, System.String) LIKE '%{palabra}%')");
                }

                string filtroFinal = string.Join(" AND ", condiciones);

                // Filtrar el DataTable
                DataRow[] filasFiltradas = dtOriginal.Select(filtroFinal);

                if (filasFiltradas.Length > 0)
                {
                    DataTable dtFiltrado = filasFiltradas.CopyToDataTable();
                    gvGrupos.DataSource = dtFiltrado;
                }
                else
                {
                    gvGrupos.DataSource = null;
                }
            }
            else
            {
                gvGrupos.DataSource = dtOriginal; // Mostrar todo si no hay búsqueda
            }

            gvGrupos.DataBind();
        }
    }
}