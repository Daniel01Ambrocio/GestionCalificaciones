using gestionescolar.BLL;
using gestionescolar.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace gestionescolar.Presentation
{
    public partial class DatosEscuela : System.Web.UI.Page
    {
        EscuelaBLL escuelaBLL = new EscuelaBLL();
        Entescuela entescuela = new Entescuela();
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
                    BindGrid();
                }
                else
                {
                    Response.Redirect("index.aspx");
                }

            }
        }
        // Lista temporal de ejemplo (simula la base de datos)
        private static List<Entescuela> listaEscuelas = new List<Entescuela>
        {
            new Entescuela { IDEscuela = 1, NombreEscuela = "Escuela Primaria 1", ClaveInstitucion = "A123", Direccion = "Calle 1", Telefono = "555-1234", Logotipo = "~/Content/logo.png", CicloEscolar = "2025-2026" },
        };
         

        // Método para enlazar datos al GridView
        private void BindGrid()
        {
            gdvEscuela.DataSource = listaEscuelas;
            gdvEscuela.DataBind();
        }

        protected void gdvEscuela_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gdvEscuela.EditIndex = e.NewEditIndex;
            BindGrid();

            // Mostrar el logotipo existente en el FileUpload
            GridViewRow row = gdvEscuela.Rows[e.NewEditIndex];
            entescuela =escuelaBLL.ObtenerEscuela(); // método que trae la escuela de la DB

            Image img = (Image)row.FindControl("imgEditLogotipo");
            if (!string.IsNullOrEmpty(entescuela.Logotipo))
            {
                img.ImageUrl = entescuela.Logotipo;
            }
            else
            {
                img.ImageUrl = "";
            }
        }

        protected void gdvEscuela_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = gdvEscuela.Rows[e.RowIndex];

            // Obtener la escuela desde la DB
            Entescuela escuela = escuelaBLL.ObtenerEscuela();

            // Actualizar los campos
            escuela.NombreEscuela = ((TextBox)row.FindControl("txtNombreEscuela")).Text;
            escuela.ClaveInstitucion = ((TextBox)row.FindControl("txtClaveInstitucion")).Text;
            escuela.Direccion = ((TextBox)row.FindControl("txtDireccion")).Text;
            escuela.Telefono = ((TextBox)row.FindControl("txtTelefono")).Text;
            escuela.CicloEscolar = ((TextBox)row.FindControl("txtCicloEscolar")).Text;

            // Manejar carga de logotipo
            FileUpload fu = (FileUpload)row.FindControl("fuLogotipo");
            if (fu.HasFile)
            {
                string extension = System.IO.Path.GetExtension(fu.FileName).ToLower();
                if (extension == ".jpg" || extension == ".jpeg" || extension == ".png")
                {
                    string fileName = "logoEscuela" + extension; // nombre fijo, ya que es una sola escuela
                    string ruta = Server.MapPath("~/Content/") + fileName;

                    // Guardar el archivo
                    fu.SaveAs(ruta);

                    // Guardar ruta relativa en la DB
                    escuela.Logotipo = "~/Content/" + fileName;
                }
                else
                {
                    // Aquí podrías mostrar un mensaje de error sobre extensión no válida
                }
            }

            // Guardar cambios en la base de datos
            string mensaje = escuelaBLL.ActualizarEscuela(escuela);
            if(mensaje == "Correcto")
            {
                MostrarAlerta("Actualización exitosa.", true);
            }
            else
            {
                MostrarAlerta("Fallo en la actualización, intentelo más tarde.", false);
            }
            gdvEscuela.EditIndex = -1;
            BindGrid();
        }

        // Evento cuando se cancela la edición
        protected void gdvEscuela_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gdvEscuela.EditIndex = -1;
            BindGrid();
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