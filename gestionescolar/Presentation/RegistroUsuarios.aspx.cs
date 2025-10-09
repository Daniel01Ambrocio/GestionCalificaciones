using gestionescolar.BLL;
using gestionescolar.DLL;
using gestionescolar.Entities;
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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Ocultar los divs al cargar la página
                OcultarCamposGenerico();
                grupoDiv.Visible = false;
                cedulaDiv.Visible = false;
                btnRegistrar.Visible = false;
                CargarRoles();
                CargarStatus();
            }
        }
        private void CargarRoles()
        {
            DataTable dt = new DataTable();
            dt = rolBLL.ObtenerRoles();
            ddlRol.DataSource = dt;
            ddlRol.DataTextField = "NombreRol";
            ddlRol.DataValueField = "IdRol";
            ddlRol.DataBind();

            // Agrega el ítem por defecto
            ddlRol.Items.Insert(0, new ListItem("Selecciona un rol", ""));
        }
        private void CargarStatus()
        {
            DataTable dt = new DataTable();
            dt = StatusBLL.ObtenerStatus();
            ddlStatus.DataSource = dt;
            ddlStatus.DataTextField = "descrípcion";
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
                string.IsNullOrWhiteSpace(ddlStatus.SelectedValue) ||
                string.IsNullOrWhiteSpace(ddlRol.SelectedValue))
            {
                MostrarMensaje("Todos los campos deben estar llenos.");
            }
            else
            {
                string valida = ValidaContraseña(txtPassword.Text, txtvalicontra.Text);
                if (valida != "Correcto")
                {
                    MostrarMensaje(valida);
                }
                else
                {
                    if (ddlRol.SelectedValue == "3") //Administrativo
                    {
                        mensaje = "";
                        entAdministrativo.Nombre = txtNombre.Text;
                        entAdministrativo.ApellidoPaterno = txtApellidoPaterno.Text;
                        entAdministrativo.ApellidoMaterno = txtApellidoMaterno.Text;
                        string contrasenia = HashPassword(txtPassword.Text);
                        entAdministrativo.contrasena = contrasenia;
                        entAdministrativo.PeriodoIngreso = Convert.ToDateTime(txtPeriodoIngreso.Text);
                        if (string.IsNullOrWhiteSpace(txtPeriodoFin.Text))
                        {
                            DateTime fechaIngreso;
                            if (DateTime.TryParse(txtPeriodoIngreso.Text, out fechaIngreso))
                            {
                                DateTime fechaFin = fechaIngreso.AddYears(1);
                                txtPeriodoFin.Text = fechaFin.ToString("yyyy-MM-dd");
                                entAdministrativo.PeriodoFin = Convert.ToDateTime(txtPeriodoFin.Text);
                            }
                        }
                        else
                        {
                            entAdministrativo.PeriodoFin = Convert.ToDateTime(txtPeriodoFin.Text);
                        }
                        entAdministrativo.IDStatus = Convert.ToInt16(ddlStatus.SelectedValue);
                        entAdministrativo.IDRol = Convert.ToInt16(ddlRol.SelectedValue);
                        mensaje = administrativoBLL.RegistrarAdministrativo(entAdministrativo);
                        LimpiarFormulario();
                        MostrarMensaje(mensaje);
                    }
                    else if (ddlRol.SelectedValue == "4") //Director
                    {
                        mensaje = "";
                        entdirector.Nombre = txtNombre.Text;
                        entdirector.ApellidoPaterno = txtApellidoPaterno.Text;
                        entdirector.ApellidoMaterno = txtApellidoMaterno.Text;
                        entdirector.contrasena = txtPassword.Text;
                        entdirector.PeriodoIngreso = Convert.ToDateTime(txtPeriodoIngreso.Text);
                        entdirector.PeriodoFin = Convert.ToDateTime(txtPeriodoFin.Text);
                        entdirector.IDStatus = Convert.ToInt16(ddlStatus.SelectedValue);
                        entdirector.IDRol = Convert.ToInt16(ddlRol.SelectedValue);
                        mensaje = directorBLL.RegistrarDirector(entdirector);
                        MostrarMensaje(mensaje);
                    }
                    else if (ddlRol.SelectedValue == "2") //Maestro
                    {
                        if (ddlGrupo.SelectedIndex <= 0 ||
                            string.IsNullOrWhiteSpace(txtCedula.Text))
                        {
                            MostrarMensaje("Todos los campos deben estar llenos.");
                        }
                        else
                        {
                            mensaje = "";
                            entmaestro.Nombre = txtNombre.Text;
                            entmaestro.ApellidoPaterno = txtApellidoPaterno.Text;
                            entmaestro.ApellidoMaterno = txtApellidoMaterno.Text;
                            entmaestro.contrasena = txtPassword.Text;
                            entmaestro.PeriodoIngreso = Convert.ToDateTime(txtPeriodoIngreso.Text);
                            entmaestro.PeriodoFin = Convert.ToDateTime(txtPeriodoFin.Text);
                            entmaestro.IDStatus = Convert.ToInt16(ddlStatus.SelectedValue);
                            entmaestro.IDRol = Convert.ToInt16(ddlRol.SelectedValue);
                            entmaestro.cedulaprofesional = txtCedula.Text;
                            entmaestro.grupo = Convert.ToInt16(ddlGrupo.SelectedValue);
                            mensaje = maestroBLL.RegistrarMaestro(entmaestro);
                            MostrarMensaje(mensaje);
                        }

                    }
                    else if (ddlRol.SelectedValue == "1") // alumno
                    {
                        if (ddlGrupo.SelectedIndex <= 0)
                        {
                            MostrarMensaje("Todos los campos deben estar llenos.");
                        }
                        else
                        {
                            mensaje = "";
                            entalumno.Nombre = txtNombre.Text;
                            entalumno.ApellidoPaterno = txtApellidoPaterno.Text;
                            entalumno.ApellidoMaterno = txtApellidoMaterno.Text;
                            entalumno.contrasena = txtPassword.Text;
                            entalumno.PeriodoIngreso = Convert.ToDateTime(txtPeriodoIngreso.Text);
                            entalumno.PeriodoFin = Convert.ToDateTime(txtPeriodoFin.Text);
                            entalumno.IDStatus = Convert.ToInt16(ddlStatus.SelectedValue);
                            entalumno.grupo = Convert.ToInt16(ddlGrupo.SelectedValue);
                            entalumno.IDRol = Convert.ToInt16(ddlRol.SelectedValue);
                            mensaje = alumnoBLL.RegistrarAlumno(entalumno);
                            MostrarMensaje(mensaje);
                        }

                    }
                    else
                    {
                        //opcion incorrecta
                        MostrarMensaje("Opción invalida");
                    }
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
            if (ddlRol.SelectedValue == "3")
            {
                MostrarCamposGenerico();
                grupoDiv.Visible = false;
                cedulaDiv.Visible = false;
                btnRegistrar.Visible = true;
            }
            else if (ddlRol.SelectedValue == "4")
            {
                MostrarCamposGenerico();
                grupoDiv.Visible = false;
                cedulaDiv.Visible = false;
                btnRegistrar.Visible = true;
            }
            else if (ddlRol.SelectedValue == "2")
            {
                MostrarCamposGenerico();
                grupoDiv.Visible = true;
                cedulaDiv.Visible = true;
                btnRegistrar.Visible = true;
            }
            else if (ddlRol.SelectedValue == "1")
            {
                MostrarCamposGenerico();
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
        protected void MostrarMensaje(string mensaje)
        {
            string script = $"alert('{mensaje}');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alerta", script, true);
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
        public string CrearNombreUser(string rol, string nombre, string fecharegistro, string apellidop, string apellidom)
        {
            try
            {
                string nombreRol = "";
                // Validar y obtener las primeras 2 letras del nombre
                string parteNombre = !string.IsNullOrWhiteSpace(nombre) && nombre.Length >= 2
                    ? nombre.Substring(0, 2)
                    : nombre;

                // Parsear la fecha
                if (!DateTime.TryParse(fecharegistro, out DateTime fecha))
                {
                    return "Fecha inválida";
                }

                // Obtener día, mes y año como texto
                string parteFecha = fecha.ToString("yyyy");

                // Dos primeras letras del apellido paterno
                string parteApellidop = !string.IsNullOrWhiteSpace(apellidop) && apellidop.Length >= 2
                    ? apellidop.Substring(0, 2)
                    : apellidop;

                // Dos primeras letras del apellido materno
                string parteApellidom = !string.IsNullOrWhiteSpace(apellidom) && apellidom.Length >= 2
                    ? apellidom.Substring(0, 2)
                    : apellidom;

                //Obtener el rol
                // 1:alumno, 2: matesro, 3:administrativo, 4:director
                switch (rol)
                {
                    case "1":
                        nombreRol = "AL";
                        break;
                    case "2":
                        nombreRol = "MA";
                        break;
                    case "3":
                        nombreRol = "AD";
                        break;
                    case "4":
                        nombreRol = "DI";
                        break;
                }

                // Concatenar todo y devolver en minúsculas (opcional)
                string nombreUsuario = (nombreRol + parteNombre + parteFecha + parteApellidop + parteApellidom).ToLower();

                return nombreUsuario;
            }
            catch
            {
                return "Error";
            }
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