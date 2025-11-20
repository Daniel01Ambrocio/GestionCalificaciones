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
    public partial class ImprimirBoleta : System.Web.UI.Page
    {
        AlumnoBLL alumnoBLL = new AlumnoBLL();
        Entgrupo entgrupo = new Entgrupo();
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
                Response.Redirect("index.aspx");   // tu página de login
            }
            if (!IsPostBack)
            {
                string usuario = Convert.ToString(Session["Usuario"]);
                string status = Convert.ToString(Session["Status"]);
                bool v = ValidarUsuario(usuario, status);
                if (v)
                {
                    //Cargar la lista de grupos que estan asignados al maestro
                    CargarGrupos();

                    btnAtras.Visible = false;
                }
                else
                {
                    Response.Redirect("index.aspx");
                }

            }
        }
        protected void ddlGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idGrupo = Convert.ToInt16(ddlGrupo.SelectedValue);
            entgrupo.IDGrupo = idGrupo;
            // Cargar alumnos según grupo
            ddlAlumno.DataSource = alumnoBLL.ObtenerAlumnosPorGrupo(entgrupo);
            ddlAlumno.DataTextField = "NombreCompleto";
            ddlAlumno.DataValueField = "Matricula";
            ddlAlumno.DataBind();

        }
        protected void rblTipoImpresion_SelectedIndexChanged(object sender, EventArgs e)
        {
            divAlumno.Visible = (rblTipoImpresion.SelectedValue == "alumno");
        }
        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            string idGrupo = ddlGrupo.SelectedValue;
            if (idGrupo == "Selecciona un grupo")
            {
                MostrarAlerta("Seleccione un grupo", false);
            }
            else
            {
                if (rblTipoImpresion.SelectedValue == "grupo")
                {
                    // Imprimir boletas de todos los alumnos del grupo
                    Response.Redirect("BoletasGrupo.aspx?grupo=" + idGrupo);
                }
                else
                {
                    // Imprimir solo un alumno
                    string idAlumno = ddlAlumno.SelectedValue;
                    if (idAlumno == "Selecciona un alumno")
                    {
                        MostrarAlerta("Seleccione un grupo", false);
                    }
                    else
                    {
                        Response.Redirect("BoletaAlumno.aspx?alumno=" + idAlumno);
                    }
                }
            }
        }
        private void CargarGrupos()
        {
            DataTable dt = grupoBLL.ObtenerGrupos();

            ddlGrupo.Items.Clear(); // Limpiar antes de cargar


            foreach (DataRow row in dt.Rows)
            {
                string texto = $"{row["grado"]}-{row["Grupo"]}-{row["anio"]}";
                string valor = row["IDGrupo"].ToString();

                ddlGrupo.Items.Add(new ListItem(texto, valor));
            }
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
    }
}