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
                entgrupo.anio = txtAnio.Text;
                mensaje = grupoBLL.RegistrarGrupo(entgrupo);
                if (mensaje == "Registro exitoso.")
                {
                    LimpiarFormulario();

                    MostrarAlerta(mensaje, true);
                    MostrarGrupos();
                }
                else
                {
                    MostrarAlerta(mensaje, false);
                }
                LimpiarFormulario();
            }
                
        }
        private void LimpiarFormulario()
        {
            txtGrado.Text = "";
            txtGrupo.Text = "";
            txtAnio.Text = "";
        }
    }
}