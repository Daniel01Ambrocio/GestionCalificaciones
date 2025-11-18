using gestionescolar.BLL;
using gestionescolar.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace gestionescolar.Presentation
{
    public partial class CambiarContrasena : System.Web.UI.Page
    {
        UsuarioBLL usuarioBLL = new UsuarioBLL();
        EntUsuario entUsuario = new EntUsuario();
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
                   
                }
                else
                {
                    Response.Redirect("index.aspx");
                }

            }
        }
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            string anterior = HashPassword(txtAnterior.Text);
            string nueva = txtNueva.Text;
            string confirmar = txtConfirmar.Text;
            string valida = ValidaContraseña(nueva, confirmar);
            if (valida != "Correcto")
            {
                MostrarAlerta(valida, false);
            }
            if (txtNueva.Text == txtAnterior.Text)
            {
                MostrarAlerta("La nueva contraseña debe ser diferente a la contraseña actual.", false);
                return;
            }

            bool validaAnterior = false;
            //Validamos que la contraseña anterior corresponda al usuario
            entUsuario.usuario = Convert.ToString(Session["Usuario"]);
            entUsuario.contrasena = anterior;
            validaAnterior = usuarioBLL.ConfirmaAnteriorContrasena(entUsuario);
            if (validaAnterior)
            {
                //realizamos la actualizacion
                bool validaActualizacion = false;
                entUsuario.contrasenaNueva = HashPassword(nueva);
                validaActualizacion = usuarioBLL.ActualizarContrasena(entUsuario);
                if(validaActualizacion)
                {
                    MostrarAlerta("Contraseña actualizada correctamente.", true);
                }
                else
                {
                    MostrarAlerta("Error en el sistema al intentar actualizar la contraseña, intentelo nuevamente más tarde.", false);
                }
            }
            else
            {
                MostrarAlerta("La contraseña anterior es incorrecta.", false);
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
                return "La contraseña debe tener al menos 7 caracteres, al menos una letra mayúscula, una letra minúscula, un número y al menos uno de los siguientes símbolos: _, @, &";
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

            // Validar al menos uno de los símbolos permitidos: _ @ &
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
    }
}