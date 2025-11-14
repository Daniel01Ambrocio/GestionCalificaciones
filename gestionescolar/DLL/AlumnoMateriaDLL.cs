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
                        // 1️⃣ Insertar AlumnoMateria
                        string queryAlumnoMateria = @"
                    INSERT INTO AlumnoMateria (Matricula, IDMateria)
                    VALUES (@Matricula, @IDMateria);
                    SELECT SCOPE_IDENTITY();"; // Obtener el ID generado

                        int nuevoIDAlumnoMateria;

                        using (SqlCommand cmd = new SqlCommand(queryAlumnoMateria, conn))
                        {
                            cmd.Parameters.AddWithValue("@Matricula", entalumno.Matricula);
                            cmd.Parameters.AddWithValue("@IDMateria", idMateria);

                            // Ejecutar y obtener el ID generado
                            nuevoIDAlumnoMateria = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        // 2️⃣ Insertar calificaciones iniciales en 0
                        string queryCalificacion = @"
                    INSERT INTO Calificacion (IDAlumnoMateria, Parcial1, Parcial2, Parcial3, Parcial4, Promedio)
                    VALUES (@IDAlumnoMateria, 0, 0, 0, 0, 0);";

                        using (SqlCommand cmdCal = new SqlCommand(queryCalificacion, conn))
                        {
                            cmdCal.Parameters.AddWithValue("@IDAlumnoMateria", nuevoIDAlumnoMateria);
                            cmdCal.ExecuteNonQuery();
                        }
                    }

                    return "Registro exitoso.";
                }
            }
            catch (SqlException ex)
            {
                return "Error al registrar la materia en la base de datos: " + ex.Message;
            }
            catch (Exception ex)
            {
                return $"Error inesperado: {ex.Message}";
            }
        }
    }
}