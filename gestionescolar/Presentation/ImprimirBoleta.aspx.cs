using gestionescolar.BLL;
using gestionescolar.Entities;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.ConstrainedExecution;
using System.Web;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace gestionescolar.Presentation
{
    public partial class ImprimirBoleta : System.Web.UI.Page
    {
        AlumnoBLL alumnoBLL = new AlumnoBLL();
        Entgrupo entgrupo = new Entgrupo();
        GrupoBLL grupoBLL = new GrupoBLL();
        Entalumno entalumno = new Entalumno();
        EscuelaBLL escuelaBLL = new EscuelaBLL();
        Entescuela entescuela = new Entescuela();
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
                Response.Redirect("index.aspx");   // tu página de login
            }
            if (!IsPostBack)
            {
                string usuario = Convert.ToString(Session["Usuario"]);
                string status = Convert.ToString(Session["Status"]);
                bool v = ValidarUsuario(usuario, status);
                if (v)
                {
                    //Cargar la lista de grupos que estan asignados al maestro
                    CargarGrupos();
                    ddlAlumno.Visible = false; // Ocultar DropDownList 
                    btnAtras.Visible = false;
                    btnImprimir.Visible = false;
                    btnSleccionarAlumno.Visible = false;
                    divAlumno.Visible = false;
                }
                else
                {
                    Response.Redirect("index.aspx");
                }

            }
        }

        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            string idGrupo = ddlGrupo.SelectedValue;
            if (idGrupo == "Selecciona un grupo")
            {
                MostrarAlerta("Seleccione un grupo", false);
                return;
            }

            int idGrupoSeleccionado;
            if (!int.TryParse(idGrupo, out idGrupoSeleccionado))
            {
                MostrarAlerta("ID de grupo no válido", false);
                return;
            }

            entgrupo.IDGrupo = idGrupoSeleccionado;
            entescuela = escuelaBLL.ObtenerEscuela();

            if (rblTipoImpresion.SelectedValue == "grupo")
            {
                DataTable dtMatriculas = alumnoBLL.ObtenerAlumnosPorGrupo(entgrupo);

                if (dtMatriculas == null || dtMatriculas.Rows.Count == 0)
                {
                    MostrarAlerta("No hay alumnos registrados en el grupo.", false);
                }
                else
                {
                    // Crear un único documento PDF
                    PdfDocument document = new PdfDocument();

                    foreach (DataRow row in dtMatriculas.Rows)
                    {
                        int matricula;
                        if (row["Matricula"] != DBNull.Value && int.TryParse(row["Matricula"].ToString(), out matricula))
                        {
                            var info = InformacionComunBoleta(matricula);
                            DataTable dtAlumno = info.dtAlumno;
                            DataTable dtAcademicos = info.dtAcademicos;

                            // Agregar la página del alumno al PDF existente
                            GenerarPaginaAlumno(document, entescuela, dtAlumno, dtAcademicos);
                        }
                    }

                    // Guardar PDF final y enviarlo al cliente
                    string ruta = HttpContext.Current.Server.MapPath("~/boletas.pdf");
                    document.Save(ruta);

                    Response.ContentType = "application/pdf";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=boletas.pdf");
                    Response.TransmitFile(ruta);
                    Response.End();
                }
                
            }
            else
            {
                // Para un solo alumno, puedes usar la misma función
                int matricula;
                if (int.TryParse(ddlAlumno.SelectedValue, out matricula))
                {
                    var info = InformacionComunBoleta(matricula);
                    DataTable dtAlumno = info.dtAlumno;
                    DataTable dtAcademicos = info.dtAcademicos;

                    PdfDocument document = new PdfDocument();
                    GenerarPaginaAlumno(document, entescuela, dtAlumno, dtAcademicos);

                    string ruta = HttpContext.Current.Server.MapPath("~/boleta.pdf");
                    document.Save(ruta);

                    Response.ContentType = "application/pdf";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=boleta.pdf");
                    Response.TransmitFile(ruta);
                    Response.End();
                }
            }
        }

        public (DataTable dtAlumno, DataTable dtAcademicos) InformacionComunBoleta(int matricula)
        {
            // Obtener los datos del alumno
            DataTable dtAlumno = alumnoBLL.ObtenerAlumnoPorMatricula(
                new  Entalumno { Matricula = matricula }
            );

            // Obtener los datos académicos del alumno
            DataTable dtAcademicos = calificacionBLL.ObtenerCalififcacionesPorMatricula(
                new Entalumno { Matricula = matricula }
            );

            // Regresamos ambos DataTable
            return (dtAlumno, dtAcademicos);
        }
        private void GenerarPaginaAlumno(PdfDocument document, Entescuela escuela, DataTable dtAlumno, DataTable dtAcademicos)
        {
            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);

            XFont titleFont = new XFont("Verdana", 16);
            XFont subFont = new XFont("Verdana", 12);
            XFont textFont = new XFont("Verdana", 10);

            int y = 40;

            // LOGO ESCUELA
            if (!string.IsNullOrEmpty(escuela.Logotipo))
            {
                string logoPath = HttpContext.Current.Server.MapPath(escuela.Logotipo);
                if (File.Exists(logoPath))
                {
                    XImage logo = XImage.FromFile(logoPath);
                    gfx.DrawImage(logo, 40, y, 90, 90);
                }
            }

            int textX = 150;
            gfx.DrawString(escuela.NombreEscuela, titleFont, XBrushes.Black, textX, y + 10);
            gfx.DrawString("Clave: " + escuela.ClaveInstitucion, textFont, XBrushes.Black, textX, y + 35);
            gfx.DrawString("Ciclo Escolar: " + escuela.CicloEscolar, textFont, XBrushes.Black, textX, y + 55);
            gfx.DrawString(escuela.Direccion, textFont, XBrushes.Black, textX, y + 75);
            if (!string.IsNullOrEmpty(escuela.Telefono))
                gfx.DrawString("Tel: " + escuela.Telefono, textFont, XBrushes.Black, textX, y + 95);

            y += 120;

            DataRow alumno = dtAlumno.Rows[0];

            // Promedio
            double promedioGeneral = dtAcademicos.Rows.Count > 0
                ? dtAcademicos.AsEnumerable().Average(r => Convert.ToDouble(r["Promedio"]))
                : 0;

            // TABLA DATOS DEL ALUMNO
            int tableX = 40;
            int tableWidth = (int)page.Width - 80;
            int rowHeight = 25;
            gfx.DrawString("DATOS DEL ALUMNO", subFont, XBrushes.Black, tableX, y - 5);
            gfx.DrawRectangle(XPens.Black, tableX, y, tableWidth, rowHeight * 3);

            int colWidth = tableWidth / 3;
            gfx.DrawLine(XPens.Black, tableX + colWidth, y, tableX + colWidth, y + rowHeight * 3);
            gfx.DrawLine(XPens.Black, tableX + colWidth * 2, y, tableX + colWidth * 2, y + rowHeight * 3);
            gfx.DrawLine(XPens.Black, tableX, y + rowHeight, tableX + tableWidth, y + rowHeight);
            gfx.DrawLine(XPens.Black, tableX, y + rowHeight * 2, tableX + tableWidth, y + rowHeight * 2);

            gfx.DrawString("Nombre:", textFont, XBrushes.Black, tableX + 5, y + 17);
            gfx.DrawString(alumno["NombreCompleto"].ToString(), textFont, XBrushes.Black, tableX + colWidth + 5, y + 17);
            gfx.DrawString("Promedio general:", textFont, XBrushes.Black, tableX + colWidth * 2 + 5, y + 17);

            gfx.DrawString("Matrícula:", textFont, XBrushes.Black, tableX + 5, y + 17 + rowHeight);
            gfx.DrawString(alumno["Matricula"].ToString(), textFont, XBrushes.Black, tableX + colWidth + 5, y + 17 + rowHeight);
            gfx.DrawString(promedioGeneral.ToString("0.0"), textFont, XBrushes.Black, tableX + colWidth * 2 + 5, y + 17 + rowHeight);

            gfx.DrawString("Grado y Grupo:", textFont, XBrushes.Black, tableX + 5, y + 17 + rowHeight * 2);
            gfx.DrawString(alumno["GradoGrupo"].ToString(), textFont, XBrushes.Black, tableX + colWidth + 5, y + 17 + rowHeight * 2);

            y += rowHeight * 3 + 40;

            // TABLA CALIFICACIONES
            gfx.DrawString("CALIFICACIONES", subFont, XBrushes.Black, tableX, y - 5);
            int headerHeight = 25;
            colWidth = tableWidth / 6;
            gfx.DrawRectangle(XPens.Black, tableX, y, tableWidth, headerHeight);
            string[] headers = { "Materia", "Parcial 1", "Parcial 2", "Parcial 3", "Parcial 4", "Promedio" };
            for (int i = 0; i < headers.Length; i++)
            {
                gfx.DrawLine(XPens.Black, tableX + colWidth * i, y, tableX + colWidth * i, y + headerHeight);
                gfx.DrawString(headers[i], textFont, XBrushes.Black, tableX + colWidth * i + 5, y + 17);
            }

            y += headerHeight;

            foreach (DataRow row in dtAcademicos.Rows)
            {
                gfx.DrawRectangle(XPens.Black, tableX, y, tableWidth, headerHeight);
                gfx.DrawString(row["NombreMateria"].ToString(), textFont, XBrushes.Black, tableX + 5, y + 17);
                gfx.DrawString(row["Parcial1"].ToString(), textFont, XBrushes.Black, tableX + colWidth + 5, y + 17);
                gfx.DrawString(row["Parcial2"].ToString(), textFont, XBrushes.Black, tableX + colWidth * 2 + 5, y + 17);
                gfx.DrawString(row["Parcial3"].ToString(), textFont, XBrushes.Black, tableX + colWidth * 3 + 5, y + 17);
                gfx.DrawString(row["Parcial4"].ToString(), textFont, XBrushes.Black, tableX + colWidth * 4 + 5, y + 17);
                gfx.DrawString(row["Promedio"].ToString(), textFont, XBrushes.Black, tableX + colWidth * 5 + 5, y + 17);

                for (int i = 1; i < 6; i++)
                    gfx.DrawLine(XPens.Black, tableX + colWidth * i, y, tableX + colWidth * i, y + headerHeight);

                y += headerHeight;

                // Salto de página si es necesario
                if (y > page.Height - 60)
                {
                    page = document.AddPage();
                    gfx = XGraphics.FromPdfPage(page);
                    y = 40;
                }
            }
        }

        private void CargarGrupos()
        {
            DataTable dt = grupoBLL.ObtenerGrupos();

            ddlGrupo.Items.Clear(); // Limpiar antes de cargar


            foreach (DataRow row in dt.Rows)
            {
                string texto = $"{row["grado"]}-{row["Grupo"]}-{row["anio"]}";
                string valor = row["IDGrupo"].ToString();

                ddlGrupo.Items.Add(new ListItem(texto, valor));
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

        protected void btnOpcion_Click(object sender, EventArgs e)
        {
            btnOpcion.Visible = false;
            ddlGrupo.Visible = false;
            
            btnAtras.Visible = true;

            lbGrupo.Text = "Grupo: " + ddlGrupo.SelectedItem.Text;
            lbtipoImpresion.Text = "Tipo de impresión:";
            // Verificar si la opción seleccionada es "alumno"
            if (rblTipoImpresion.SelectedValue == "alumno")
            {
                ddlAlumno.Visible = true; // Mostrar DropDownList de alumnos
                ddlAlumno.Items.Clear();
                lbtipoImpresion.Text = "Tipo de impresión: Por alumno.";
                int idGrupo = Convert.ToInt16(ddlGrupo.SelectedValue);
                entgrupo.IDGrupo = idGrupo;

                // Cargar alumnos según grupo
                ddlAlumno.DataSource = alumnoBLL.ObtenerAlumnosPorGrupo(entgrupo);
                ddlAlumno.DataTextField = "NombreCompleto";
                ddlAlumno.DataValueField = "Matricula";
                ddlAlumno.DataBind();
                divAlumno.Visible = true;
                btnSleccionarAlumno.Visible = true;
                rblTipoImpresion.Visible = false; // Ocultar después de usar su valor
            }
            else
            {
                lbtipoImpresion.Text = "Tipo de impresión: Por grupo.";
                btnImprimir.Visible = true;
                ddlAlumno.Visible = false; // Ocultar DropDownList si no es "alumno"
                btnSleccionarAlumno.Visible = false;
                rblTipoImpresion.Visible = false;
                divAlumno.Visible = false;
            }
        }



        protected void btnAtras_Click(object sender, EventArgs e)
        {
            btnAtras.Visible = false;
            btnOpcion.Visible = true;
            ddlGrupo.Visible = true;
            ddlAlumno.Visible= false;
            btnImprimir.Visible = false;
            rblTipoImpresion.Visible = true;
            lbGrupo.Text = "Grupo:";
            lbAlumno.Text = "Alumno:";
            divAlumno.Visible = false;
            lbtipoImpresion.Text = "Tipo de impresión:";
            btnSleccionarAlumno.Visible = false;
        }

        protected void btnSleccionarAlumno_Click(object sender, EventArgs e)
        {
            lbAlumno.Text = "Alumno: " + ddlAlumno.SelectedItem.Text;
            btnImprimir.Visible = true;
            ddlAlumno.Visible = false;
            btnSleccionarAlumno.Visible = false;
        }
    }
}