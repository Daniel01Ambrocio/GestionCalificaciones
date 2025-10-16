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
    public partial class RegistrarMateria : System.Web.UI.Page
    {
        MateriaBLL materiaBLL = new MateriaBLL();
        Entmateria entmateria = new Entmateria();
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
                    MostrarMaterias();
                }
                else
                {
                    Response.Redirect("index.aspx");
                }

            }
        }
        public void MostrarMaterias()
        {
            DataTable dataMaterias = new DataTable();
            dataMaterias = materiaBLL.ObtenerMaterias();
            gvMaterias.DataSource = dataMaterias;
            gvMaterias.DataBind();
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
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtGradoEscolar.Text))
            {
                MostrarAlerta("Todos los campos deben estar llenos.", false);
            }
            else
            {
                entmateria.Nombre = txtNombre.Text;
                entmateria.GradoEscolar = Convert.ToInt16(txtGradoEscolar.Text);
                mensaje = materiaBLL.RegistrarMateria(entmateria);
                if (mensaje == "Registro exitoso.")
                {
                    LimpiarFormulario();
                    MostrarMaterias();
                    Session["mensaje"] = mensaje;
                    Response.Redirect(Request.RawUrl);
                }
                else
                {
                    MostrarAlerta(mensaje, false);
                }
            }

            entmateria = new Entmateria();
        }
        protected void gvMaterias_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string mensaje = "";
            entmateria.IDMateria = Convert.ToInt32(gvMaterias.DataKeys[e.RowIndex].Value);
            mensaje = materiaBLL.EliminarMateria(entmateria);
            if (mensaje == "Eliminación exitosa.")
            {
                LimpiarFormulario();
                MostrarMaterias();
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
            txtNombre.Text = "";
            txtGradoEscolar.Text = "";
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            string textoBusqueda = txtFiltro.Text.Trim();

            DataTable dtOriginal = materiaBLL.ObtenerMaterias(); // Tu método para cargar los Materias

            if (!string.IsNullOrEmpty(textoBusqueda))
            {
                // Separar por palabras
                string[] palabras = textoBusqueda.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                // Armar condiciones por cada palabra (en grado, Grupo, anio)
                List<string> condiciones = new List<string>();
                foreach (string palabra in palabras)
                {
                    condiciones.Add($"(CONVERT(GradoEscolar, System.String) LIKE '%{palabra}%' OR Nombre LIKE '%{palabra}%')");
                }

                string filtroFinal = string.Join(" AND ", condiciones);

                // Filtrar el DataTable
                DataRow[] filasFiltradas = dtOriginal.Select(filtroFinal);

                if (filasFiltradas.Length > 0)
                {
                    DataTable dtFiltrado = filasFiltradas.CopyToDataTable();
                    gvMaterias.DataSource = dtFiltrado;
                }
                else
                {
                    gvMaterias.DataSource = null;
                }
            }
            else
            {
                gvMaterias.DataSource = dtOriginal; // Mostrar todo si no hay búsqueda
            }

            gvMaterias.DataBind();
        }
    }
}