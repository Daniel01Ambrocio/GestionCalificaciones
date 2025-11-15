using gestionescolar.BLL;
using gestionescolar.DLL;
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
    public partial class ListaEditAlumnos : System.Web.UI.Page
    {
        AlumnoBLL alumnoBLL = new AlumnoBLL();
        Entgrupo entgrupo = new Entgrupo();
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
                    MostrarAlumno();
                }
                else
                {
                    Response.Redirect("index.aspx");
                }

            }
        }
        public void MostrarAlumno()
        {
            DataTable dataAlumnos = new DataTable();
            dataAlumnos = alumnoBLL.ObtenerAlumnos();
            gvAlumnos.DataSource = dataAlumnos;
            gvAlumnos.DataBind();
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            string textoBusqueda = txtFiltro.Text.Trim();

            DataTable dtOriginal = alumnoBLL.ObtenerAlumnos(); //método para cargar los datos

            if (!string.IsNullOrEmpty(textoBusqueda))
            {
                // Separar por palabras
                string[] palabras = textoBusqueda.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                // Armar condiciones por cada palabra
                List<string> condiciones = new List<string>();
                foreach (string palabra in palabras)
                {
                    condiciones.Add($"Nombre LIKE '%{palabra}%' OR ApellidoPaterno LIKE '%{palabra}%' OR  ApellidoMaterno LIKE '%{palabra}%' OR ApellidoMaterno LIKE '%{palabra}%' OR  Grupo LIKE '%{palabra}%' OR Estatus LIKE '%{palabra}%' OR PeriodoIngreso LIKE '%{palabra}%' OR PeriodoFin LIKE '%{palabra}%'");
                }

                string filtroFinal = string.Join(" AND ", condiciones);

                // Filtrar el DataTable
                DataRow[] filasFiltradas = dtOriginal.Select(filtroFinal);

                if (filasFiltradas.Length > 0)
                {
                    DataTable dtFiltrado = filasFiltradas.CopyToDataTable();
                    gvAlumnos.DataSource = dtFiltrado;
                }
                else
                {
                    gvAlumnos.DataSource = null;
                }
            }
            else
            {
                gvAlumnos.DataSource = dtOriginal; // Mostrar todo si no hay búsqueda
            }

            gvAlumnos.DataBind();
        }
    }
}