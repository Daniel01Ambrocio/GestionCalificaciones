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