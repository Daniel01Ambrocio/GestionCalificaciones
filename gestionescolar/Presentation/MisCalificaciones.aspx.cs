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
    public partial class MisCalificaciones : System.Web.UI.Page
    {
        Entalumno entalumno = new Entalumno();
        AlumnoBLL alumnoBLL = new AlumnoBLL();
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
             
            if (!IsPostBack)
            {
                string usuario = Convert.ToString(Session["Usuario"]);
                string status = Convert.ToString(Session["Status"]);
                bool v = ValidarUsuario(usuario, status);
                if (v)
                {
                    entalumno.IDUsuario =Convert.ToInt16(Session["UsuarioID"]);
                    //Mostramos calificaciones del aluno por iDusuario
                    MostrarCalificaciones(entalumno);
                }
                else
                {
                    Response.Redirect("index.aspx");


                }

            }
        }
        public void MostrarCalificaciones(Entalumno entalumno)
        {
            DataTable dataCalificacionAlumno = new DataTable();
            dataCalificacionAlumno = alumnoBLL.MostrarCalificaciones(entalumno);
            gvCalificaciones.DataSource = dataCalificacionAlumno;
            gvCalificaciones.DataBind();
        }
    }
}