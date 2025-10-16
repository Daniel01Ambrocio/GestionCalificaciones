using gestionescolar.BLL;
using gestionescolar.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace gestionescolar.DLL
{
    public class GrupoDLL
    {
        string connectionString = ConfigurationManager.ConnectionStrings["cnn"].ConnectionString;
         
        public DataTable ObtenerGrupos()
        {
            DataTable dtRoles = new DataTable();

            string query = "SELECT IDGrupo, grado, Grupo, anio FROM grupo";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                da.Fill(dtRoles);
            }

            return dtRoles;
        }
        public int ObtenerGradoPorIdGrupo(Entgrupo entgrupo)
        {
            try
            {
                int anio = 0;

                string query = @"
                SELECT grado 
                FROM grupo 
                WHERE IDGrupo = @idGrupo";

                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@idGrupo", entgrupo.IDGrupo);

                    conn.Open();
                    object result = cmd.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int parsedAnio))
                    {
                        anio = parsedAnio;
                    }
                }

                return anio;
            }
            catch (SqlException ex)
            {
                // Puedes registrar el error aquí si tienes un sistema de logging
                return 0;
            }
        }

        public DataTable ObtenerGruposConID()
        {
            DataTable dtRoles = new DataTable();

            string query = "SELECT * FROM grupo";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                da.Fill(dtRoles);
            }

            return dtRoles;
        }
        public string RegistrarGrupo(Entgrupo grupo)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                INSERT INTO grupo (grado, Grupo, anio) 
                VALUES (@grado, @Grupo, @anio);";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@grado", grupo.grado);
                    cmd.Parameters.AddWithValue("@Grupo", grupo.grupo);
                    cmd.Parameters.AddWithValue("@anio", grupo.anio);

                    conn.Open();
                    int filasAfectadas = cmd.ExecuteNonQuery();

                    if (filasAfectadas > 0)
                    {
                        return "Registro exitoso.";
                    }
                    else
                    {
                        return "No se pudo registrar el grupo.";
                    }
                }
            }
            catch (SqlException ex)
            {
                return $"Error al registrar el grupo en la base de datos.";
            }
            catch (Exception ex)
            {
                return $"Error inesperado: {ex.Message}";
            }
        }
        public string EliminarGrupo(Entgrupo grupo)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"DELETE FROM Grupo WHERE IDGrupo = @IdGrupo";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@IdGrupo", grupo.IDGrupo);

                        conn.Open();
                        int filasAfectadas = cmd.ExecuteNonQuery();

                        if (filasAfectadas > 0)
                        {
                            return "Eliminación exitosa.";
                        }
                        else
                        {
                            return "No se pudo eliminar el grupo. Puede estar referenciado en otra tabla.";
                        }
                    }
                }
            }
            catch (SqlException)
            {
                return "Error al eliminar el grupo. Verifique si está relacionado con otros registros.";
            }
            catch (Exception ex)
            {
                return $"Error inesperado: {ex.Message}";
            }
        }

    }
}