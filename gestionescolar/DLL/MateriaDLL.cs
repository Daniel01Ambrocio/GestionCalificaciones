using gestionescolar.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Web;

namespace gestionescolar.DLL
{
    public class MateriaDLL
    {
        string connectionString = ConfigurationManager.ConnectionStrings["cnn"].ConnectionString;

        public DataTable ObtenerMaterias()
        {
            DataTable dtRoles = new DataTable();

            string query = "SELECT * FROM Materia";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                da.Fill(dtRoles);
            }

            return dtRoles;
        }
        public List<int> ObtenerMateriasPorGrado(Entgrupo entgrupo)
        {
            List<int> listaMaterias = new List<int>();
            DataTable dtRoles = new DataTable();

            string query = "SELECT IDMateria FROM Materia WHERE GradoEscolar = @grado";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@grado", entgrupo.grado);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dtRoles);
                }
            }

            foreach (DataRow row in dtRoles.Rows)
            {
                if (row["IDMateria"] != DBNull.Value)
                {
                    listaMaterias.Add(Convert.ToInt32(row["IDMateria"]));
                }
            }

            return listaMaterias;
        }

        public DataTable ObtenerMateriasConID()
        {
            DataTable dtRoles = new DataTable();

            string query = "SELECT * FROM Materia";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                da.Fill(dtRoles);
            }

            return dtRoles;
        }
        public string RegistrarMateria(Entmateria Materia)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                INSERT INTO Materia (Nombre, GradoEscolar) 
                VALUES (@Nombre, @GradoEscolar);";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Nombre", Materia.Nombre);
                    cmd.Parameters.AddWithValue("@GradoEscolar", Materia.GradoEscolar);

                    conn.Open();
                    int filasAfectadas = cmd.ExecuteNonQuery();

                    if (filasAfectadas > 0)
                    {
                        return "Registro exitoso.";
                    }
                    else
                    {
                        return "No se pudo registrar el Materia.";
                    }
                }
            }
            catch (SqlException ex)
            {
                return $"Error al registrar el Materia en la base de datos.";
            }
            catch (Exception ex)
            {
                return $"Error inesperado: {ex.Message}";
            }
        }
        public string EliminarMateria(Entmateria Materia)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"DELETE FROM Materia WHERE IDMateria = @IdMateria";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@IdMateria", Materia.IDMateria);

                        conn.Open();
                        int filasAfectadas = cmd.ExecuteNonQuery();

                        if (filasAfectadas > 0)
                        {
                            return "Eliminación exitosa.";
                        }
                        else
                        {
                            return "No se pudo eliminar el Materia. Puede estar referenciado en otra tabla.";
                        }
                    }
                }
            }
            catch (SqlException)
            {
                return "Error al eliminar el Materia. Verifique si está relacionado con otros registros.";
            }
            catch (Exception ex)
            {
                return $"Error inesperado: {ex.Message}";
            }
        }
    }
}