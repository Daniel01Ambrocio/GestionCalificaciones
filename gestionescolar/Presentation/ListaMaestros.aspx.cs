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
    public partial class ListaMaestros : System.Web.UI.Page
    {
        MaestroBLL maestroBLL = new MaestroBLL();
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
                    MostrarMaestros();
                }
                else
                {
                    Response.Redirect("index.aspx");
                }

            }
        }
        public void MostrarMaestros()
        {
            DataTable dataMaestros = new DataTable();
            dataMaestros = maestroBLL.ObtenerMaestros();
            gvMaestros.DataSource = dataMaestros;
            gvMaestros.DataBind();
        }
    }
}