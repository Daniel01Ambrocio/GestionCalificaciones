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
    public class CalificacionDLL
    {
        string connectionString = ConfigurationManager.ConnectionStrings["cnn"].ConnectionString;
        public DataTable MostrarCalificaciones(Entalumno entalumno)
        {
            DataTable dtCalificaciones = new DataTable();

            string query = @"
                SELECT 
                    m.Nombre + ' ' + CAST(m.GradoEscolar AS VARCHAR(10)) AS Nombre,
                    c.Parcial1,
                    c.Parcial2,
                    c.Parcial3,
                    c.Parcial4,
                    c.Promedio
                FROM Alumno al
                INNER JOIN AlumnoMateria am ON al.Matricula = am.Matricula
                INNER JOIN Calificacion c ON c.IDAlumnoMateria = am.IDAlumnoMateria
                INNER JOIN Materia m ON m.IDMateria = am.IDMateria
                WHERE al.IDUsuario = @IDUsuario;
                ";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@IDUsuario", entalumno.IDUsuario);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dtCalificaciones);
                }
            }

            return dtCalificaciones;
        }
        public DataTable MostrarAlumnosCalificaciones(Entgrupo entgrupo)
        {
            DataTable dtCalificaciones = new DataTable();

            string query = @"
                SELECT
                c.IDCalificacion,
                u.Nombre + ' ' + u.ApellidoPaterno + ' ' + u.ApellidoMaterno AS NombreAlumno,
	            CAST(g.grado AS VARCHAR(10)) + ' ' + g.grupo + ' ' + g.anio AS Grupo,
                m.Nombre + ' ' + CAST(m.GradoEscolar AS VARCHAR(10)) AS Materia,
                c.Parcial1,
                c.Parcial2,
                c.Parcial3,
                c.Parcial4,
                c.Promedio
            FROM Alumno al
            INNER JOIN grupo g ON al.IDGrupo = g.IDGrupo
            INNER JOIN AlumnoMateria am ON al.Matricula = am.Matricula
            INNER JOIN Materia m ON m.IDMateria = am.IDMateria
            INNER JOIN Calificacion c ON c.IDAlumnoMateria = am.IDAlumnoMateria
            INNER JOIN Usuario u ON al.IDUsuario = u.IdUsuario
            WHERE al.IDGrupo = @IDGrupo";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@IDGrupo", entgrupo.IDGrupo);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dtCalificaciones);
                }
            }

            return dtCalificaciones;
        }
        public bool ActualizarCalificaciones(Entcalificacion entcalificacion)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {

                try
                {
                    string query = "UPDATE Calificacion SET Parcial1 = @Parcial1, Parcial2 = @Parcial2, Parcial3 = @Parcial3, Parcial4 = @Parcial4, Promedio = @Promedio WHERE IDCalificacion = @IDCalificacion";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@IDCalificacion", entcalificacion.IDCalificacion);
                    cmd.Parameters.AddWithValue("@Parcial1", entcalificacion.Parcial1);
                    cmd.Parameters.AddWithValue("@Parcial2", entcalificacion.Parcial2);
                    cmd.Parameters.AddWithValue("@Parcial3", entcalificacion.Parcial3);
                    cmd.Parameters.AddWithValue("@Parcial4", entcalificacion.Parcial4);
                    cmd.Parameters.AddWithValue("@Promedio", entcalificacion.Promedio);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    return true;//Realizo la actualizacion
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}