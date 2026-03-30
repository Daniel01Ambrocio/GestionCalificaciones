using gestionescolar.BLL;
using gestionescolar.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace gestionescolar.DLL
{
    public class EscuelaDLL
    {
        string connectionString = ConfigurationManager.ConnectionStrings["cnn"].ConnectionString;
        public Entescuela ObtenerEscuela()
        {
            Entescuela escuela = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT TOP 1 NombreEscuela, ClaveInstitucion, Direccion, Telefono, Logotipo, CicloEscolar FROM Escuela";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    // Buscar el archivo real en /Content/
                    string basePath = HttpContext.Current.Server.MapPath("~/Content/");
                    string logoName = reader["Logotipo"] != DBNull.Value ? reader["Logotipo"].ToString() : null;
                    string logoPath = null;

                    if (!string.IsNullOrEmpty(logoName))
                    {
                        string[] posiblesExtensiones = { ".jpg", ".jpeg", ".png" };

                        foreach (string ext in posiblesExtensiones)
                        {
                            string fullPath = Path.Combine(basePath, logoName + ext);
                            if (File.Exists(fullPath))
                            {
                                logoPath = "/Content/" + logoName + ext;
                                break;
                            }
                        }
                    }

                    escuela = new Entescuela
                    {
                        NombreEscuela = reader["NombreEscuela"].ToString(),
                        ClaveInstitucion = reader["ClaveInstitucion"].ToString(),
                        Direccion = reader["Direccion"].ToString(),
                        Telefono = reader["Telefono"] != DBNull.Value ? reader["Telefono"].ToString() : null,
                        Logotipo = logoPath, // ← Aquí queda el ImageUrl correcto
                        CicloEscolar = reader["CicloEscolar"].ToString()
                    };
                }
                reader.Close();
            }

            return escuela;
        }
        public int ObtenerIDEscuela(Entescuela entescuela)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
            SELECT IDEscuela 
            FROM Escuela
            WHERE NombreEscuela = @NombreEscuela
              AND ClaveInstitucion = @ClaveInstitucion
              AND Direccion = @Direccion
              AND (@Telefono IS NULL OR Telefono = @Telefono)
              AND (@Logotipo IS NULL OR Logotipo = @Logotipo)
              AND CicloEscolar = @CicloEscolar";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@NombreEscuela", entescuela.NombreEscuela);
                cmd.Parameters.AddWithValue("@ClaveInstitucion", entescuela.ClaveInstitucion);
                cmd.Parameters.AddWithValue("@Direccion", entescuela.Direccion);
                cmd.Parameters.AddWithValue("@Telefono", (object)entescuela.Telefono ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Logotipo", (object)entescuela.Logotipo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@CicloEscolar", entescuela.CicloEscolar);

                conn.Open();
                object result = cmd.ExecuteScalar();

                return result != null ? Convert.ToInt32(result) : 0;
            }
        }

        public string ActualizarEscuela(Entescuela escuela)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"UPDATE Escuela SET 
                             NombreEscuela=@NombreEscuela, 
                             ClaveInstitucion=@ClaveInstitucion, 
                             Direccion=@Direccion, 
                             Telefono=@Telefono, 
                             Logotipo=@Logotipo, 
                             CicloEscolar=@CicloEscolar
                             WHERE IDEscuela=1";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@IDEscuela", escuela.IDEscuela);
                    cmd.Parameters.AddWithValue("@NombreEscuela", escuela.NombreEscuela);
                    cmd.Parameters.AddWithValue("@ClaveInstitucion", escuela.ClaveInstitucion);
                    cmd.Parameters.AddWithValue("@Direccion", escuela.Direccion);
                    cmd.Parameters.AddWithValue("@Telefono", (object)escuela.Telefono ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Logotipo", (object)escuela.Logotipo ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@CicloEscolar", escuela.CicloEscolar);

                    conn.Open();
                    int filasAfectadas = cmd.ExecuteNonQuery();

                    if (filasAfectadas > 0)
                        return "Correcto";
                    else
                        return "Error"; // No se actualizó ninguna fila
                }
            }
            catch (Exception ex)
            {
                // Aquí podrías loguear el error si quieres
                return "Error";
            }
        }
        public string ActualizarEscuelaSinLogo(Entescuela escuela)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"UPDATE Escuela SET 
                             NombreEscuela=@NombreEscuela, 
                             ClaveInstitucion=@ClaveInstitucion, 
                             Direccion=@Direccion, 
                             Telefono=@Telefono,
                             CicloEscolar=@CicloEscolar
                             WHERE IDEscuela=1";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@IDEscuela", escuela.IDEscuela);
                    cmd.Parameters.AddWithValue("@NombreEscuela", escuela.NombreEscuela);
                    cmd.Parameters.AddWithValue("@ClaveInstitucion", escuela.ClaveInstitucion);
                    cmd.Parameters.AddWithValue("@Direccion", escuela.Direccion);
                    cmd.Parameters.AddWithValue("@Telefono", (object)escuela.Telefono ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@CicloEscolar", escuela.CicloEscolar);

                    conn.Open();
                    int filasAfectadas = cmd.ExecuteNonQuery();

                    if (filasAfectadas > 0)
                        return "Correcto";
                    else
                        return "Error"; // No se actualizó ninguna fila
                }
            }
            catch (Exception ex)
            {
                // Aquí podrías loguear el error si quieres
                return "Error";
            }
        }
    }
}