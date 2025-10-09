using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace gestionescolar.Presentation
{
    public partial class menu : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lbUsuario.Text = "Usuario: " + Convert.ToString(Session["Usuario"]);
                lbRol.Text = "Rol: " + Convert.ToString(Session["Rol"]);
            }
        }
    }
}