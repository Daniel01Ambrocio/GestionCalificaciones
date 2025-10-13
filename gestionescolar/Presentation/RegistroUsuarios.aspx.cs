using gestionescolar.BLL;
using gestionescolar.DLL;
using gestionescolar.Entities;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace gestionescolar.Presentation
{
    public partial class RegistroUsuarios : System.Web.UI.Page
    {
        Entadministrativo entAdministrativo = new Entadministrativo();
        Entdirector entdirector = new Entdirector();
        Entmaestro entmaestro = new Entmaestro();
        Entalumno entalumno = new Entalumno();
        AdministrativoBLL administrativoBLL = new AdministrativoBLL();
        DirectorBLL directorBLL = new DirectorBLL();
        MaestroBLL maestroBLL = new MaestroBLL();
        AlumnoBLL alumnoBLL = new AlumnoBLL();
        RolBLL rolBLL = new RolBLL();
        StatusBLL StatusBLL = new StatusBLL();
        GrupoBLL GrupoBLL = new GrupoBLL();
        EntUsuario entUsuario = new EntUsuario();
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
            string ms = Convert.ToString(Session["mensaje"]);
            if (ms.Length > 0)
            {
                MostrarAlerta(ms, true);
                Session["mensaje"] = "";
            }
            if (!IsPostBack)
            {
                string usuario = Convert.ToString(Session["Usuario"]);
                string status =Convert.ToString(Session["Status"]);
                bool v = ValidarUsuario(usuario, status);
                if (v)
                {
                    // Ocultar los divs al cargar la página
                    OcultarCamposGenerico();
                    grupoDiv.Visible = false;
                    cedulaDiv.Visible = false;
                    btnRegistrar.Visible = false;
                    CargarRoles();
                    CargarStatus();
                }
                else
                {
                    Response.Redirect("index.aspx");
                }
                    
            }
        }
        private void CargarRoles()
        {
            DataTable dt = new DataTable();
            ddlRol.Items.Clear(); // Limpiar antes de cargar
            dt = rolBLL.ObtenerRoles();
            ddlRol.DataSource = dt;
            ddlRol.DataTextField = "NombreRol";
            ddlRol.DataValueField = "IdRol";
            ddlRol.DataBind();

            // Agrega el ítem por defecto
            ddlRol.Items.Insert(0, new ListItem("Selecciona un rol", ""));
        }
        private void CargarGrupos()
        {
            DataTable dt = GrupoBLL.ObtenerGruposConID();

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

        private void CargarStatus()  
        {
            DataTable dt = new DataTable();
            ddlStatus.Items.Clear(); // Limpiar antes de cargar
            dt = StatusBLL.ObtenerStatus();
            ddlStatus.DataSource = dt;
            ddlStatus.DataTextField = "descripcion";
            ddlStatus.DataValueField = "IDStatus";
            ddlStatus.DataBind();
            // Eliminar el último ítem del DropDownList = estatus = egresado
            if (ddlStatus.Items.Count > 0)
            {
                ddlStatus.Items.RemoveAt(ddlStatus.Items.Count - 1);
            }

            // Agrega el ítem por defecto
            ddlStatus.Items.Insert(0, new ListItem("Selecciona un estatus", ""));
        }

        protected void btnRegistrar_Click(object sender, EventArgs e)
        {
            string mensaje = "";
            string opcion = ddlRol.SelectedValue;

            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtApellidoPaterno.Text) ||
                string.IsNullOrWhiteSpace(txtApellidoMaterno.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text) ||
                string.IsNullOrWhiteSpace(txtvalicontra.Text) ||
                string.IsNullOrWhiteSpace(txtPeriodoIngreso.Text) ||
                string.IsNullOrWhiteSpace(txtPeriodoFin.Text) ||
                string.IsNullOrWhiteSpace(ddlStatus.SelectedValue)) 
            {
                MostrarAlerta("Todos los campos deben estar llenos.", false);
            }
            else
            {
                DateTime fechainicio = Convert.ToDateTime(txtPeriodoIngreso.Text);
                DateTime fechafin = Convert.ToDateTime(txtPeriodoFin.Text);
                if(fechafin > fechainicio)
                {
                    string valida = ValidaContraseña(txtPassword.Text, txtvalicontra.Text);
                    if (valida != "Correcto")
                    {
                        MostrarAlerta(valida, false);
                    }
                    else
                    {
                        string pass = "";
                        pass = HashPassword(txtPassword.Text);
                        entUsuario.Nombre = txtNombre.Text;
                        entUsuario.ApellidoPaterno = txtApellidoPaterno.Text;
                        entUsuario.ApellidoMaterno = txtApellidoMaterno.Text;
                        entUsuario.contrasena = pass;
                        entUsuario.PeriodoIngreso = Convert.ToDateTime(txtPeriodoIngreso.Text);
                        entUsuario.PeriodoFin = Convert.ToDateTime(txtPeriodoFin.Text);
                        entUsuario.IDStatus = Convert.ToInt16(ddlStatus.SelectedValue);
                        entUsuario.IDROL = Convert.ToInt16(ddlRol.SelectedValue);

                        if (ddlRol.SelectedValue == "3") //Administrativo
                        {
                            mensaje = "";
                            mensaje = administrativoBLL.RegistrarAdministrativo(entUsuario);


                        }
                        else if (ddlRol.SelectedValue == "4") //Director
                        {
                            mensaje = "";
                            mensaje = directorBLL.RegistrarDirector(entUsuario);

                        }
                        else if (ddlRol.SelectedValue == "2") //Maestro
                        {
                            //validamos que el campo de cedula y de grupo estén llenos
                            if (ddlGrupo.SelectedIndex <= 0 ||
                                string.IsNullOrWhiteSpace(txtCedula.Text))
                            {
                                MostrarAlerta("Todos los campos deben estar llenos.", false);
                            }
                            else
                            {
                                mensaje = "";
                                entmaestro.cedulaprofesional = txtCedula.Text;
                                entmaestro.IDGrupo = Convert.ToInt16(ddlGrupo.SelectedValue);
                                mensaje = maestroBLL.RegistrarMaestro(entUsuario, entmaestro);

                            }

                        }
                        else if (ddlRol.SelectedValue == "1") // alumno
                        {
                            if (ddlGrupo.SelectedIndex <= 0) //validamos que el campo de grupo esté lleno
                            {
                                MostrarAlerta("Todos los campos deben estar llenos.", false);
                            }
                            else
                            {
                                mensaje = "";
                                entalumno.IDGrupo = Convert.ToInt16(ddlGrupo.SelectedValue);
                                mensaje = alumnoBLL.RegistrarAlumno(entUsuario ,entalumno);
                            }

                        }
                        else
                        {
                            //opcion incorrecta
                            MostrarAlerta("Opción invalida", false);
                        }
                        if (mensaje == "Registro exitoso.")
                        {
                            LimpiarFormulario();
                            Session["mensaje"] = mensaje;
                            Response.Redirect(Request.RawUrl);
                        }
                        else
                        {
                            MostrarAlerta(mensaje, false);
                        }
                    }
                }
                else
                {
                    MostrarAlerta("El periodo de fin debe de ser mayor al periodo de inicio.", false);
                }
                    
            }
        }

        protected void btnRol_Click(object sender, EventArgs e)
        {
            // Ocultar los divs
            OcultarCamposGenerico();
            grupoDiv.Visible = false;
            cedulaDiv.Visible = false;
            btnRegistrar.Visible = false;
            string opcion = ddlRol.SelectedValue;
            if (ddlRol.SelectedValue == "3")//administrativo
            {
                MostrarCamposGenerico();
                grupoDiv.Visible = false;
                cedulaDiv.Visible = false;
                btnRegistrar.Visible = true;
            }
            else if (ddlRol.SelectedValue == "4")//Director
            {
                MostrarCamposGenerico();
                grupoDiv.Visible = false;
                cedulaDiv.Visible = false;
                btnRegistrar.Visible = true;
            }
            else if (ddlRol.SelectedValue == "2")//maestro
            {
                MostrarCamposGenerico();
                CargarGrupos();
                grupoDiv.Visible = true;
                cedulaDiv.Visible = true;
                btnRegistrar.Visible = true;
            }
            else if (ddlRol.SelectedValue == "1")//alumno
            {
                MostrarCamposGenerico();
                CargarGrupos();
                grupoDiv.Visible = true;
                cedulaDiv.Visible = false;
                btnRegistrar.Visible = true;
            }
            else
            {
                // Ocultar los divs
                OcultarCamposGenerico();
                grupoDiv.Visible = false;
                cedulaDiv.Visible = false;
                btnRegistrar.Visible = false;
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

        public void LimpiarFormulario()
        {
            ddlRol.SelectedIndex = -1;
            txtNombre.Text = "";
            txtApellidoPaterno.Text = "";
            txtApellidoMaterno.Text = "";
            txtvalicontra.Text = "";
            txtPassword.Text = "";
            txtPeriodoIngreso.Text = "";
            txtPeriodoFin.Text = "";
            ddlStatus.SelectedIndex = -1;
            ddlGrupo.SelectedIndex = -1;
            txtCedula.Text = "";

        }
        public void MostrarCamposGenerico()
        {
            nombrediv.Visible = true;
            apellidopdiv.Visible = true;
            apellidomdiv.Visible = true;
            contravalidiv.Visible = true;
            contradiv.Visible = true;
            periodoinidiv.Visible = true;
            periodofindiv.Visible = true;
            statusdiv.Visible = true;
            contrainfodiv.Visible = true;
        }
        public void OcultarCamposGenerico()
        {
            nombrediv.Visible = false;
            apellidopdiv.Visible = false;
            apellidomdiv.Visible = false;
            contravalidiv.Visible = false;
            contradiv.Visible = false;
            periodoinidiv.Visible = false;
            periodofindiv.Visible = false;
            statusdiv.Visible = false;
            contrainfodiv.Visible = false;
        }
        
        public string ValidaContraseña(string contra, string validaContra)
        {
            string mensaje = "";

            if (contra != validaContra)
            {
                return "Las contraseñas no coinciden.";
            }

            // Validar longitud mínima
            if (contra.Length < 7)
            {
                return "La contraseña debe tener al menos 7 caracteres.";
            }

            // Validar al menos una letra mayúscula
            if (!contra.Any(char.IsUpper))
            {
                return "La contraseña debe contener al menos una letra mayúscula.";
            }

            // Validar al menos una letra minúscula
            if (!contra.Any(char.IsLower))
            {
                return "La contraseña debe contener al menos una letra minúscula.";
            }

            // Validar al menos un número
            if (!contra.Any(char.IsDigit))
            {
                return "La contraseña debe contener al menos un número.";
            }

            // Validar al menos uno de los símbolos permitidos: _ @ 6
            if (!contra.Contains("_") && !contra.Contains("@") && !contra.Contains("&"))
            {
                return "La contraseña debe contener al menos uno de los siguientes símbolos: _, @, &";
            }
            if (mensaje == "")// Si no hubo errores, mensaje queda vacío (Correcto)
            {
                mensaje = "Correcto";
            }
            return mensaje;
        }
        //hashear contraseña
        private string HashPassword(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);

                // Convertir el hash a string hexadecimal
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x3"));
                }

                return sb.ToString();
            }
        }
    }
}