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
                    (CAST(g.grado AS VARCHAR) + '-' + g.grupo + '-' + CAST(g.anio AS VARCHAR)) AS Grupo, 
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
        public string ObtenerAlumPorFullNamYGrupo(EntUsuario entUsuario, Entalumno entalumno)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                SELECT COUNT(*)
                FROM Alumno a
                INNER JOIN Usuario u ON u.IdUsuario = a.IDUsuario
                WHERE u.Nombre = @Nombre
                  AND u.ApellidoPaterno = @ApellidoPaterno
                  AND u.ApellidoMaterno = @ApellidoMaterno
                  AND a.IDGrupo = @IDGrupo";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Nombre", entUsuario.Nombre);
                        cmd.Parameters.AddWithValue("@ApellidoPaterno", entUsuario.ApellidoPaterno);
                        cmd.Parameters.AddWithValue("@ApellidoMaterno", entUsuario.ApellidoMaterno);
                        cmd.Parameters.AddWithValue("@IDGrupo", entalumno.IDGrupo);

                        conn.Open();

                        int cantidad = Convert.ToInt32(cmd.ExecuteScalar());

                        return cantidad > 0 ? "Existe" : "No existe";
                    }
                }
            }
            catch (SqlException)
            {
                return "Error";
            }
        }
        public string ActualizarGrupoAlumno(Entalumno alumnoent)
        {
            string respuesta = "";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "UPDATE Alumno SET IDGrupo = @IDGrupo WHERE Matricula = @Matricula";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@IDGrupo", alumnoent.IDGrupo);
                        cmd.Parameters.AddWithValue("@Matricula", alumnoent.Matricula);

                        int filasAfectadas = cmd.ExecuteNonQuery();

                        if (filasAfectadas > 0)
                        {
                            respuesta = "Actualización correcta.";
                        }
                        else
                        {
                            respuesta = "No se encontró el alumno.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                respuesta = "Error.";
            }

            return respuesta;
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
        public DataTable ObtenerAlumnoPorMatricula(Entalumno entalumno)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"SELECT
                    u.Nombre + ' ' + u.ApellidoPaterno + ' ' + u.ApellidoMaterno AS NombreCompleto,
                    a.Matricula,
                    (CAST(g.grado AS VARCHAR) + ' ' + g.grupo) AS GradoGrupo
                FROM Alumno a
                INNER JOIN Usuario u ON a.IDUsuario = u.IDUsuario
                INNER JOIN Grupo g ON a.IDGrupo = g.IDGrupo
                WHERE a.Matricula = @matricula";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@matricula", entalumno.Matricula);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }

            return dt;
        }
        public int CantidadAlumnosEngrupo(Entgrupo entgrupo)
        {
            try
            {
                int cantidad = 0;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"select count(*) from Alumno where IDGrupo=@IDGrupo";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@IDGrupo", entgrupo.IDGrupo);

                        conn.Open();
                        object result = cmd.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int parsedMatricula))
                        {
                            cantidad = parsedMatricula;
                        }
                    }
                }

                return cantidad;
            }
            catch (SqlException ex)
            {
                //error
                return 1000;
            }
        }
    }
}