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
    public partial class ListaEditAlumnos : System.Web.UI.Page
    {
        AlumnoBLL alumnoBLL = new AlumnoBLL();
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
    }
}