using gestionescolar.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace gestionescolar.DLL
{
    public class UsuarioDLL
    {
        string connectionString = ConfigurationManager.ConnectionStrings["cnn"].ConnectionString;
        public int RegistrarUsuario(EntUsuario entUsuario)
        {
            try
            {
                int IDUsuario = 0;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                INSERT INTO Usuario 
                (Nombre, ApellidoPaterno, ApellidoMaterno, contrasena, PeriodoIngreso, PeriodoFin, IDStatus, IDROL) 
                VALUES 
                (@Nombre, @ApellidoPaterno, @ApellidoMaterno, @contrasena, @PeriodoIngreso, @PeriodoFin, @status, @rol);
                SELECT SCOPE_IDENTITY();";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Nombre", entUsuario.Nombre);
                    cmd.Parameters.AddWithValue("@ApellidoPaterno", entUsuario.ApellidoPaterno);
                    cmd.Parameters.AddWithValue("@ApellidoMaterno", entUsuario.ApellidoMaterno);
                    cmd.Parameters.AddWithValue("@contrasena", entUsuario.contrasena);
                    cmd.Parameters.AddWithValue("@PeriodoIngreso", entUsuario.PeriodoIngreso);
                    cmd.Parameters.AddWithValue("@PeriodoFin", entUsuario.PeriodoFin);
                    cmd.Parameters.AddWithValue("@status", entUsuario.IDStatus);
                    cmd.Parameters.AddWithValue("@rol", entUsuario.IDROL);

                    conn.Open();
                    object result = cmd.ExecuteScalar(); // Devuelve el ID insertado

                    if (result != null)
                    {
                        int.TryParse(result.ToString(), out IDUsuario);
                    }
                }

                return IDUsuario;
            }
            catch (SqlException ex)
            {
                return 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public bool ActualizaUser(EntUsuario entUsuario)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {

                try
                {
                    string query = "UPDATE Usuario SET usuario = @usuario WHERE IDUsuario = @id";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@usuario", entUsuario.usuario);
                    cmd.Parameters.AddWithValue("@id", entUsuario.IdUsuario);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}