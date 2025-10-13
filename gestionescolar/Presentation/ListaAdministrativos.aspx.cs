using gestionescolar.BLL;
using gestionescolar.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace gestionescolar.Presentation
{
    public partial class ListaAdministrativos : System.Web.UI.Page
    {
       AdministrativoBLL administrativoBLL = new AdministrativoBLL();
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
            if (!IsPostBack)
            {
                string usuario = Convert.ToString(Session["Usuario"]);
                string status = Convert.ToString(Session["Status"]);
                bool v = ValidarUsuario(usuario, status);
                if (v)
                {
                    MostrarAdministrativos();
                }
                else
                {
                    Response.Redirect("index.aspx");
                }

            }
        }
        public void MostrarAdministrativos()
        {
            DataTable dataAdministrativos = new DataTable();
            dataAdministrativos = administrativoBLL.ObtenerAdministrativos();
            gvAdministrativos.DataSource = dataAdministrativos;
            gvAdministrativos.DataBind();
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            string textoBusqueda = txtFiltro.Text.Trim();

            DataTable dtOriginal = administrativoBLL.ObtenerAdministrativos(); //método para cargar los datos

            if (!string.IsNullOrEmpty(textoBusqueda))
            {
                // Separar por palabras
                string[] palabras = textoBusqueda.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                // Armar condiciones por cada palabra
                List<string> condiciones = new List<string>();
                foreach (string palabra in palabras)
                {
                    condiciones.Add($"Nombre LIKE '%{palabra}%' OR ApellidoPaterno LIKE '%{palabra}%' OR  ApellidoMaterno LIKE '%{palabra}%' OR ApellidoMaterno LIKE '%{palabra}%' OR Estatus LIKE '%{palabra}%' OR PeriodoIngreso LIKE '%{palabra}%' OR PeriodoFin LIKE '%{palabra}%'");
                }

                string filtroFinal = string.Join(" AND ", condiciones);

                // Filtrar el DataTable
                DataRow[] filasFiltradas = dtOriginal.Select(filtroFinal);

                if (filasFiltradas.Length > 0)
                {
                    DataTable dtFiltrado = filasFiltradas.CopyToDataTable();
                    gvAdministrativos.DataSource = dtFiltrado;
                }
                else
                {
                    gvAdministrativos.DataSource = null;
                }
            }
            else
            {
                gvAdministrativos.DataSource = dtOriginal; // Mostrar todo si no hay búsqueda
            }

            gvAdministrativos.DataBind();
        }
    }
}