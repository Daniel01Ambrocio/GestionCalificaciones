using gestionescolar.BLL;
using gestionescolar.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;

namespace gestionescolar.DLL
{
    public class GrupoDLL
    {
        string connectionString = ConfigurationManager.ConnectionStrings["cnn"].ConnectionString;
         
        public DataTable ObtenerGrupos()
        {
            DataTable dtGrupos = new DataTable();

            string query = "SELECT IDGrupo, grado, Grupo, anio FROM grupo";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                da.Fill(dtGrupos);
            }

            return dtGrupos;
        }
        public DataTable ObtenerGruposDelPeriodo(int periodo)
        {
            DataTable dtGrupos = new DataTable();
            try
            { 
                string query = @"
                SELECT IDGrupo, grado, grupo, anio
                FROM grupo 
                WHERE anio = @anio";

                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@anio", periodo);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtGrupos);
                    }
                }

                return dtGrupos;
            }
            catch (SqlException ex)
            {
                 
                return dtGrupos;
            }
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
                //0 = no hay
                return 0;
            }
        }

        public DataTable ObtenerGruposConID()
        {
            DataTable dtGrupos = new DataTable();

            string query = "SELECT * FROM grupo";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                da.Fill(dtGrupos);
            }

            return dtGrupos;
        }
        public DataTable CargarGruposPorMaestro(EntUsuario entUsuario)
        {
            DataTable dtGrupos = new DataTable();

            // Corregir la sintaxis de la consulta SQL
            string query = "SELECT g.IDGrupo, g.grado, g.grupo, g.anio FROM grupo AS g INNER JOIN maestro AS m ON g.idgrupo = m.idgrupo INNER JOIN usuario AS u ON m.idusuario = u.idusuario WHERE u.usuario = @usuario";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                // Agregar el parámetro para evitar inyecciones SQL
                cmd.Parameters.AddWithValue("@usuario", entUsuario.usuario);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dtGrupos);
                }
            }

            return dtGrupos;
        }

        public string RegistrarGrupo(Entgrupo entgrupo)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                INSERT INTO grupo (grado, Grupo, anio) 
                VALUES (@grado, @Grupo, @anio);";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@grado", entgrupo.grado);
                    cmd.Parameters.AddWithValue("@Grupo", entgrupo.grupo);
                    cmd.Parameters.AddWithValue("@anio", entgrupo.anio);

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
        public int RegistrarGrupoObtenerIDGrupo(Entgrupo entgrupo)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                INSERT INTO grupo (grado, Grupo, anio) 
                VALUES (@grado, @Grupo, @anio);
                SELECT SCOPE_IDENTITY();";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@grado", entgrupo.grado);
                    cmd.Parameters.AddWithValue("@Grupo", entgrupo.grupo);
                    cmd.Parameters.AddWithValue("@anio", entgrupo.anio);

                    conn.Open();

                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        return Convert.ToInt32(result);
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            catch (SqlException)
            {
                return 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public string EliminarGrupo(Entgrupo entgrupo)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"DELETE FROM Grupo WHERE IDGrupo = @IdGrupo";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@IdGrupo", entgrupo.IDGrupo);

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
        public bool ExisteAlumnoEnGrupo(Entgrupo entgrupo)
        {
            int count = 1;
            int idgrupo = entgrupo.IDGrupo;
            //select count(*) from Alumno where IDGrupo = 1
            using(SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "select count(*) from Alumno where IDGrupo = @IDGrupo";
                
                using(SqlCommand cmd= new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IdGrupo", entgrupo.IDGrupo);
                    conn.Open();
                    count=(int)cmd.ExecuteScalar();
                }
            }
            if(count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public int BuscarExistenciaGrupo(Entgrupo entgrupo)
        {
            int idGrupo = 0;

            string query = @"SELECT idGrupo 
                     FROM Grupo 
                     WHERE grado = @grado 
                     AND grupo = @grupo 
                     AND anio = @anio;";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@grado", SqlDbType.Int).Value = entgrupo.grado;
                    cmd.Parameters.Add("@grupo", SqlDbType.VarChar).Value = entgrupo.grupo;
                    cmd.Parameters.Add("@anio", SqlDbType.Int).Value = entgrupo.anio;

                    conn.Open();

                    object result = cmd.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        idGrupo = Convert.ToInt32(result);
                    }
                }
            }
            catch (SqlException ex)
            {
                idGrupo=0; 
            }

            return idGrupo;
        }
    }
}