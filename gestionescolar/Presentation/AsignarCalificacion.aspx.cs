using gestionescolar.BLL;
using gestionescolar.DLL;
using gestionescolar.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace gestionescolar.Presentation
{
    public partial class AsignarCalificacion : System.Web.UI.Page
    {
        GrupoBLL grupoBLL = new GrupoBLL();
        EntUsuario entUsuario = new EntUsuario();
        Entgrupo entgrupo = new Entgrupo();
        AlumnoBLL alumnoBLL = new AlumnoBLL();
        Entcalificacion entcalificacion = new Entcalificacion();
        CalificacionBLL calificacionBLL = new CalificacionBLL();
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
                Response.Redirect("Login.aspx");
            }
            if (!IsPostBack)
            {
                string usuario = Convert.ToString(Session["Usuario"]);
                string status = Convert.ToString(Session["Status"]);
                bool v = ValidarUsuario(usuario, status);
                if (v)
                {
                    //Cargar la lista de grupos que estan asignados al maestro
                    CargarGruposPorMaestro();
                    btnAtras.Visible = false;
                    //Cargar la lista de alumnos segun el grupo seleccionado
                    MostrarAlumnosCalificaciones(entgrupo);
                }
                else
                {
                    Response.Redirect("Login.aspx");
                }

            }
        }
        public void MostrarAlumnosCalificaciones(Entgrupo entgrupo)
        {
            DataTable dataAlumnos = new DataTable();
            dataAlumnos = calificacionBLL.MostrarAlumnosCalificaciones(entgrupo);
            gdvAlumnoCalificaciones.DataSource = dataAlumnos;
            gdvAlumnoCalificaciones.DataBind();
        }
        private void CargarGruposPorMaestro()
        {
            entUsuario.usuario = Convert.ToString(Session["Usuario"]);
            DataTable dt = grupoBLL.CargarGruposPorMaestro(entUsuario);

            ddlGrupo.Items.Clear(); // Limpiar antes de cargar

            // Agrega el ítem por defecto
            ddlGrupo.Items.Add(new ListItem("Selecciona un grupo", ""));

            foreach (DataRow row in dt.Rows)
            {
                string texto = $"{row["grado"]}-{row["Grupo"]}-{row["anio"]}";
                string valor = row["IDGrupo"].ToString();

                ddlGrupo.Items.Add(new ListItem(texto, valor));
            }
        }

        protected void btngrupo_Click(object sender, EventArgs e)
        {
            // Verificar que se haya seleccionado un grupo
            if (ddlGrupo.SelectedValue != "")
            {
                // Mostrar el grupo seleccionado en el label (solo texto, sin editar)
                lbGrupo.Text = "Grupo seleccionado: " + ddlGrupo.SelectedItem.Text;

                // Hacer invisible el DropDownList para mostrar solo el Label
                ddlGrupo.Visible = false;
                btngrupo.Visible = false;
                // Obtener el ID del grupo seleccionado y asignarlo a entgrupo
                entgrupo.IDGrupo = Convert.ToInt16(ddlGrupo.SelectedValue);
                btnAtras.Visible = true;
                // Llamar al método para mostrar alumnos y calificaciones
                MostrarAlumnosCalificaciones(entgrupo);
                
            }
            else
            {
                // Mostrar un mensaje de error o advertencia si no se ha seleccionado un grupo
                MostrarAlerta("Por favor, selecciona un grupo.", false);
            }
        }
        protected void gdvAlumnoCalificaciones_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gdvAlumnoCalificaciones.EditIndex = e.NewEditIndex;
            entgrupo.IDGrupo = Convert.ToInt16(ddlGrupo.SelectedValue);
            // Llamar al método para mostrar alumnos y calificaciones
            MostrarAlumnosCalificaciones(entgrupo);
        }

        protected void gdvAlumnoCalificaciones_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gdvAlumnoCalificaciones.EditIndex = -1;
            entgrupo.IDGrupo = Convert.ToInt16(ddlGrupo.SelectedValue);
            // Llamar al método para mostrar alumnos y calificaciones
            MostrarAlumnosCalificaciones(entgrupo);
        }

        protected void gdvAlumnoCalificaciones_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int index = e.RowIndex;
            GridViewRow row = gdvAlumnoCalificaciones.Rows[index];

            // Recuperar ID del registro
            int IDCalificacion = Convert.ToInt32(gdvAlumnoCalificaciones.DataKeys[index].Value);

            // Obtener valores editados
            string parcial1 = ((TextBox)row.Cells[3].Controls[0]).Text;
            string parcial2 = ((TextBox)row.Cells[4].Controls[0]).Text;
            string parcial3 = ((TextBox)row.Cells[5].Controls[0]).Text;
            string parcial4 = ((TextBox)row.Cells[6].Controls[0]).Text;

            decimal c1, c2, c3, c4;

            // Validar que sean números
            bool valid1 = decimal.TryParse(parcial1, out c1);
            bool valid2 = decimal.TryParse(parcial2, out c2);
            bool valid3 = decimal.TryParse(parcial3, out c3);
            bool valid4 = decimal.TryParse(parcial4, out c4);

            if (!valid1 || !valid2 || !valid3 || !valid4)
            {
                MostrarAlerta("Todos los valores deben ser numéricos.",false);
            }
            else
            {
                c1 = Convert.ToDecimal(parcial1);
                c2 = Convert.ToDecimal(parcial2);
                c3 = Convert.ToDecimal(parcial3);
                c4 = Convert.ToDecimal(parcial4);
                if (c1 >= 0 && c1 <= 10)
                {
                    if (c2 >= 0 && c2 <= 10)
                    {
                        if (c3 >= 0 && c3 <= 10)
                        {
                            if (c4 >= 0 && c4 <= 10)
                            {
                                //Realizar la actualización de las calififcaciones
                                entcalificacion.IDCalificacion = IDCalificacion;
                                entcalificacion.Parcial1 = c1;
                                entcalificacion.Parcial2 = c2;
                                entcalificacion.Parcial3 = c3;
                                entcalificacion.Parcial4 = c4;
                                decimal promedio = (c1 + c2 + c3 + c4) / 4;
                                entcalificacion.Promedio = promedio;
                                bool validaActualizacion = calificacionBLL.ActualizarCalificaciones(entcalificacion);
                                if (validaActualizacion)
                                {
                                    MostrarAlerta("Actualización exitosa.", true);
                                }
                                else
                                {
                                    MostrarAlerta("Error al actualizar. Intentelo más tarde.", true);
                                }
                            }
                            else
                            {
                                MostrarAlerta("La calificación del parcial 4 debe ser un valor entre 0 y 10.", false);
                            }
                        }
                        else
                        {
                            MostrarAlerta("La calificación del parcial 3 debe ser un valor entre 0 y 10.", false);
                        }
                    }
                    else
                    {
                        MostrarAlerta("La calificación del parcial 2 debe ser un valor entre 0 y 10.", false);
                    }
                }
                else
                {
                    MostrarAlerta("La calificación del parcial 1 debe ser un valor entre 0 y 10.", false);
                }
            }
            gdvAlumnoCalificaciones.EditIndex = -1;
            entgrupo.IDGrupo = Convert.ToInt16(ddlGrupo.SelectedValue);
            // Llamar al método para mostrar alumnos y calificaciones
            MostrarAlumnosCalificaciones(entgrupo);
        }


        protected void btnAtras_Click(object sender, EventArgs e)
        {
            ddlGrupo.Visible = true;
            btngrupo.Visible = true;
            btnAtras.Visible = false;
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