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
    public partial class ListaDirectores : System.Web.UI.Page
    {
        DirectorBLL directorBLL = new DirectorBLL();
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
                    MostrarDirectores();
                }
                else
                {
                    Response.Redirect("index.aspx");
                }

            }
        }
        public void MostrarDirectores()
        {
            DataTable dataDirectores = new DataTable();
            dataDirectores = directorBLL.ObtenerDirectores();
            gvDirectores.DataSource = dataDirectores;
            gvDirectores.DataBind();
        }
    }
}