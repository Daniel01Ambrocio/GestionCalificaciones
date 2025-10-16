using gestionescolar.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace gestionescolar.DLL
{
    public class AlumnoMateriaDLL
    {
        string connectionString = ConfigurationManager.ConnectionStrings["cnn"].ConnectionString;
        public string RegistrarAlumnoMateria(List<int> listaIdMateria, Entalumno entalumno)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    foreach (int idMateria in listaIdMateria)
                    {
                        string query = @"
                    INSERT INTO AlumnoMateria (Matricula, IDMateria)
                    VALUES (@Matricula, @IDMateria);";

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@Matricula", entalumno.Matricula);
                            cmd.Parameters.AddWithValue("@IDMateria", idMateria);

                            int filasAfectadas = cmd.ExecuteNonQuery();

                            if (filasAfectadas <= 0)
                            {
                                return $"No se pudo registrar la materia con ID {idMateria} para el alumno.";
                            }
                        }
                    }

                    return "Registro exitoso.";
                }
            }
            catch (SqlException ex)
            {
                return "Error al registrar el Materia en la base de datos.";
            }
            catch (Exception ex)
            {
                return $"Error inesperado: {ex.Message}";
            }
        }

    }
}