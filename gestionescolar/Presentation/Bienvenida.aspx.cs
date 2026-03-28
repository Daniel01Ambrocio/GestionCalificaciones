using gestionescolar.Entities;
using NewsAPI;
using NewsAPI.Constants;
using NewsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace gestionescolar.Presentation
{
    public partial class Bienvenida : System.Web.UI.Page
    {
        private static readonly HttpClient client = new HttpClient();
        private readonly string apiKey = "2cff8bf74e4741f39ec1e9d81f31bfd1"; // Mantén la clave en una constante.
        NoticiasEnt news = new NoticiasEnt();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Carga la fecha del sistema

                if (Session["FechaSeleccionada"] != null)
                {
                    DateTime fechaGuardada = (DateTime)Session["FechaSeleccionada"];
                    calFecha.SelectedDate = fechaGuardada;  // Establecemos la fecha seleccionada
                    calFecha.VisibleDate = fechaGuardada;   // Para mantener la vista del calendario en la misma fecha
                }
                else
                {
                    // Si no hay fecha en sesión, usamos la fecha actual
                    calFecha.SelectedDate = DateTime.Today;
                    calFecha.VisibleDate = DateTime.Today;
                }
                gdvNoticias.Visible = false; // No mostrar la GridView al principio
            }
        }


        // Método refactorizado
        public async Task Prueba(string categoria, DateTime? fecha)
        {
            Session["FechaSeleccionada"] = fecha;
            Session["categoria"] = categoria;
            try
            {
                // Inicia el cliente de NewsAPI
                var newsApiClient = new NewsApiClient(apiKey);

                // Realiza la consulta utilizando la categoría como parámetro
                var articlesResponse = await newsApiClient.GetEverythingAsync(new EverythingRequest
                {
                    Q = categoria, // Usamos la categoría para la búsqueda
                    SortBy = SortBys.Popularity,
                    Language = Languages.EN,
                    From = fecha ?? new DateTime(2025, 7, 25) // Usar fecha seleccionada, si no, la fecha predeterminada
                });

                // Verifica que la respuesta sea exitosa
                if (articlesResponse.Status == Statuses.Ok)
                {
                    // Limpiamos el DataGridView antes de agregar los nuevos datos
                    gdvNoticias.DataSource = null;  // Limpiar DataSource
                    gdvNoticias.DataBind();

                    // Itera sobre los artículos y agrega los datos al DataGridView
                    var articlesList = articlesResponse.Articles.Select(article => new
                    {
                        Titulo = article.Title,
                        Autor = article.Author,
                        Enlace = article.Url,
                        Publicado = article.PublishedAt
                    }).ToList();

                    if (articlesList.Any())
                    {
                        gdvNoticias.DataSource = articlesList;
                        gdvNoticias.DataBind();
                        gdvNoticias.Visible = true;
                        lblMensaje.Visible = false;
                    }
                    else
                    {
                        MostrarMensaje("No se encontraron artículos para esta categoría.");
                    }
                }
                else
                {
                    MostrarMensaje("Error al obtener artículos: " + articlesResponse.Status);
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje($"Error: {ex.Message}");
            }
        }

        // Método para mostrar mensaje de error
        private void MostrarMensaje(string mensaje)
        {
            gdvNoticias.Visible = false;
            lblMensaje.Visible = true;
            lblMensaje.Text = mensaje;
        }




        // Eventos asíncronos de botones
        protected async void btnMostrarProgramacion_Click(object sender, EventArgs e)
        {
            string categoria = "Programación";
            DateTime? fechaSeleccionada = calFecha.SelectedDate == DateTime.MinValue ? (DateTime?)null : calFecha.SelectedDate;
            await Prueba(categoria, fechaSeleccionada);
        }

        protected async void btnMostrarNASA_Click(object sender, EventArgs e)
        {
            string categoria = "NASA";
            DateTime? fechaSeleccionada = calFecha.SelectedDate == DateTime.MinValue ? (DateTime?)null : calFecha.SelectedDate;
            await Prueba(categoria, fechaSeleccionada);
        }

        protected async void btnMostrarEducacion_Click(object sender, EventArgs e)
        {
            string categoria = "Educacion";

            DateTime? fechaSeleccionada = calFecha.SelectedDate == DateTime.MinValue ? (DateTime?)null : calFecha.SelectedDate;
            await Prueba(categoria, fechaSeleccionada);
        }
    }
}