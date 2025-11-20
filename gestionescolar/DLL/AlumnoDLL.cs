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
    public class AlumnoDLL
    {
        string connectionString = ConfigurationManager.ConnectionStrings["cnn"].ConnectionString;
        public DataTable ObtenerAlumnos()
        {
            DataTable dtalumno = new DataTable();

            string query = @"SELECT 
                    u.Nombre, 
                    u.ApellidoPaterno, 
                    u.ApellidoMaterno, 
                    (CAST(g.grado AS VARCHAR) + '-' + g.grupo + '-' + g.anio) AS Grupo, 
                    e.descripcion as Estatus, 
                    FORMAT(u.PeriodoIngreso, 'dd-MM-yyyy') AS PeriodoIngreso, 
                    FORMAT(u.PeriodoFin, 'dd-MM-yyyy') AS PeriodoFin
                FROM Alumno a
                INNER JOIN Usuario u ON a.IDUsuario = u.IDUsuario
                INNER JOIN Grupo g ON a.IDGrupo = g.IDGrupo
                INNER JOIN Estatus e ON u.IDStatus = e.IDStatus";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                da.Fill(dtalumno);
            }

            return dtalumno;
        }
        public string RegistrarAlumno(EntUsuario entUsuario, Entalumno entalumno)
        {
            try
            {
                int iddic = 0;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                    INSERT INTO Alumno 
                    (IDGrupo, IDUsuario) 
                    VALUES 
                    (@IDGrupo,@IDUsuario);
                    SELECT SCOPE_IDENTITY();";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@IDGrupo", entalumno.IDGrupo);
                    cmd.Parameters.AddWithValue("@IDUsuario", entUsuario.IdUsuario);

                    conn.Open();
                    object result = cmd.ExecuteScalar(); // Devuelve el ID insertado

                    if (result != null)
                    {
                        int.TryParse(result.ToString(), out iddic);
                        return "AL" + iddic;
                    }
                    else
                    {
                        return "Error de registro";
                    }
                }

            }
            catch (SqlException ex)
            {
                return "Error de registro";
            }
        }
        public int BuscarMatriculaByUsuario(EntUsuario entUsuario)
        {
            try
            {
                int matricula = 0;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                SELECT a.matricula 
                FROM Alumno AS a
                INNER JOIN Usuario u ON u.IdUsuario = a.IDUsuario
                WHERE u.usuario = @usuario";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@usuario", entUsuario.usuario);

                        conn.Open();
                        object result = cmd.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int parsedMatricula))
                        {
                            matricula = parsedMatricula;
                        }
                    }
                }

                return matricula;
            }
            catch (SqlException ex)
            {
                //error
                return 0;
            }
        }
        public DataTable ObtenerAlumnosPorGrupo(Entgrupo entgrupo)
        {
            DataTable dtalumno = new DataTable();

            string query = @"SELECT 
                        (u.Nombre +' '+ u.ApellidoPaterno +' '+ u.ApellidoMaterno) AS NombreCompleto,
                        a.Matricula
                     FROM Alumno a
                     INNER JOIN Usuario u ON a.IDUsuario = u.IDUsuario
                     INNER JOIN Grupo g ON a.IDGrupo = g.IDGrupo
                     WHERE a.IDGrupo = @IDGrupo";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                cmd.Parameters.AddWithValue("@IDGrupo", entgrupo.IDGrupo);

                conn.Open();
                da.Fill(dtalumno);
            }

            return dtalumno;
        }

    }
}